// Scroll via data attributes — no Blazor JS interop needed
document.addEventListener('click', function (e) {
    var btn = e.target.closest('[data-scroll-to]');
    if (!btn) return;
    var id = btn.getAttribute('data-scroll-to');
    if (!id) return;
    var el = document.getElementById(id);
    if (el) el.scrollIntoView({ behavior: 'smooth', block: 'start' });
});
