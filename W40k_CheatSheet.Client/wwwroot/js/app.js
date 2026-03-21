window.scrollToEl = function (id) {
    try {
        var el = document.getElementById(id);
        if (el) el.scrollIntoView({ behavior: 'smooth', block: 'start' });
    } catch (e) { }
};
