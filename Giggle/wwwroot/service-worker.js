const CACHE_NAME = 'blog-cache-v1';
const urlsToCache = [
  '/',
  '/index.php',

	'/vendor/bootstrap/css/bootstrap.min.css',
	'/vendor/bootstrap-icons/bootstrap-icons.css',
	'/vendor/aos/aos.css',
	'/vendor/glightbox/css/glightbox.min.css',
	'/vendor/swiper/swiper-bundle.min.css',

	'/css/main.css',
	'/css/custom.css',



	'/css/custom.css',
	'/css/custom.css',


  '/vendor/bootstrap/js/bootstrap.bundle.min.js',
	'/vendor/aos/aos.js',
	'/vendor/glightbox/js/glightbox.min.js',
	'/vendor/purecounter/purecounter_vanilla.js',
	'/vendor/swiper/swiper-bundle.min.js',
	'/vendor/php-email-form/validate.js',
	'/js/main.js',


  '/img/pwa/icon-48x48.png',
  '/img/pwa/icon-72x72.png',
  '/img/pwa/icon-96x96.png',
  '/img/pwa/icon-144x144.png',
  '/img/pwa/icon-192x192.png',
  '/img/pwa/icon-256x256.png',
  '/img/pwa/icon-384x384.png',
  '/img/pwa/icon-512x512.png',
]
self.addEventListener('install', event => {
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then(cache => {
        return cache.addAll(urlsToCache);
      })
  );
});

self.addEventListener('fetch', event => {
  event.respondWith(
    caches.match(event.request)
      .then(response => {
        if (response) {
          return response;
        }
        return fetch(event.request);
      })
  );
});
