const VALIDATION_TIMEOUT = 2000;

function debounce(func, delay) {
    let timeout;
    return function (...args) {
        clearTimeout(timeout);
        timeout = setTimeout(() => func.apply(this, args), delay);
    };
}

function displayError(element, message, isError) {
    if (isError) {
        element.textContent = message;
        element.classList.add('d-block');
        element.classList.remove('d-none');
    } else {
        element.classList.add('d-none');
        element.classList.remove('d-block');
    }
}

function toggleValidationClasses(inputElement, isValid) {
    if (isValid) {
        inputElement.classList.add('is-valid');
        inputElement.classList.remove('is-invalid');
    } else {
        inputElement.classList.add('is-invalid');
        inputElement.classList.remove('is-valid');
    }
}

function validateInput(selector, errorSelector, validationFunc, checkUrl, callback) {
    const inputElement = document.querySelector(selector);
    const errorElement = document.querySelector(errorSelector);

    inputElement.addEventListener('input', debounce(function () {
        const value = inputElement.value;
        const isValid = validationFunc(value);

        if (isValid) {
            $.ajax({
                url: checkUrl,
                type: 'GET',
                data: { value: value },
                success: function (response) {
                    const isUnique = response.isUnique;
                    displayError(errorElement, 'Value is already taken.', !isUnique);
                    toggleValidationClasses(inputElement, isUnique);
                    callback();
                },
                error: function () {
                    displayError(errorElement, 'An error occurred. Please try again later.', true);
                }
            });
        } else {
            if (value !== '') {
                displayError(errorElement, 'Invalid input.', true);
            } else {
                displayError(errorElement, '', false);
            }
            toggleValidationClasses(inputElement, false);
        }
        callback();
    }, VALIDATION_TIMEOUT));
}

function validateEmailInput(emailSelector, emailErrorSelector, emailRegexPattern, checkEmailUrl, callback) {
    const emailRegex = new RegExp(emailRegexPattern);
    validateInput(emailSelector, emailErrorSelector, (value) => emailRegex.test(value), checkEmailUrl, callback);
}

function validateUsernameInput(usernameSelector, usernameErrorSelector, checkUsernameUrl, updateSubmitButtonState) {
    validateInput(usernameSelector, usernameErrorSelector, (value) => value.length >= 3, checkUsernameUrl, updateSubmitButtonState);
}

function validateNameInput(nameSelector, updateSubmitButtonState) {
    document.querySelectorAll(nameSelector).forEach(function (input) {
        input.addEventListener('input', function () {
            const isValid = this.value.length >= 2;
            toggleValidationClasses(this, isValid);
            displayError(this.nextElementSibling, 'Please enter at least 2 characters.', !isValid);
            updateSubmitButtonState();
        });
    });
}

function validatePasswordInput(passwordSelector, passwordErrorSelector, passwordRegexPattern, updateSubmitButtonState) {
    const passwordInput = document.querySelector(passwordSelector);
    const passwordError = document.querySelector(passwordErrorSelector);
    const passwordRegex = new RegExp(passwordRegexPattern);

    passwordInput.addEventListener('input', function () {
        const password = passwordInput.value;
        const isValid = passwordRegex.test(password);
        toggleValidationClasses(passwordInput, isValid);
        displayError(passwordError, 'Password must be at least 8 characters and include upper/lower case letters, numbers, and symbols.', !isValid);
        updateSubmitButtonState();
    });
}

function validatePasswordRepeatInput(passwordSelector, passwordRepeatSelector, passwordRepeatErrorSelector, updateSubmitButtonState) {
    const passwordInput = document.querySelector(passwordSelector);
    const passwordRepeatInput = document.querySelector(passwordRepeatSelector);
    const passwordRepeatError = document.querySelector(passwordRepeatErrorSelector);

    passwordRepeatInput.addEventListener('input', function () {
        const password = passwordInput.value;
        const confirmPassword = passwordRepeatInput.value;
        const isValid = confirmPassword === password && confirmPassword.length > 0;
        toggleValidationClasses(passwordRepeatInput, isValid);
        displayError(passwordRepeatError, 'Passwords do not match.', !isValid);
        updateSubmitButtonState();
    });
}

function validateTermsInput(termsSelector, updateSubmitButtonState) {
    const termsInput = document.querySelector(termsSelector);

    termsInput.addEventListener('change', function () {
        const isValid = this.checked;
        toggleValidationClasses(termsInput, isValid);
        updateSubmitButtonState();
    });
}

function initializeFormValidationAndSubmission(formSelector, submitUrl, successRedirectUrl, updateSubmitButtonState) {
    const forms = document.querySelectorAll(formSelector);
    forms.forEach(function (form) {
        form.addEventListener('submit', function (event) {
            event.preventDefault();
            if (!form.checkValidity()) {
                event.stopPropagation();
            } else {
                const formData = new FormData(form);
                const submitButton = form.querySelector(':submit');

                submitButton.disabled = true;
                submitButton.textContent = 'Submitting...';
                submitButton.classList.add('loading-btn');

                $.ajax({
                    url: submitUrl,
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (response) {
                        Swal.fire({
                            title: response.success ? 'Success!' : 'Error!',
                            text: response.message,
                            icon: response.success ? 'success' : 'error',
                            confirmButtonText: 'OK'
                        }).then((result) => {
                            if (result.isConfirmed && response.success) {
                                window.location.href = successRedirectUrl;
                            }
                        });
                        submitButton.disabled = false;
                        submitButton.textContent = 'Sign Up';
                        submitButton.classList.remove('loading-btn');
                    },
                    error: function () {
                        Swal.fire({
                            title: 'Error!',
                            text: 'Form submission failed. Please try again.',
                            icon: 'error',
                            confirmButtonText: 'OK'
                        });
                        submitButton.disabled = false;
                        submitButton.textContent = 'Sign Up';
                        submitButton.classList.remove('loading-btn');
                    }
                });
            }
            form.classList.add('was-validated');
        }, false);
    });
    updateSubmitButtonState();
}
