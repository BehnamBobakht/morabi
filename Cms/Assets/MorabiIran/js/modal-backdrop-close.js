(function () {
    'use strict';

    function visibleModal() {
        return document.querySelector('.modal.show, .modal.in, .modal[style*="display: block"]');
    }

    function closeModal(modal) {
        if (!modal) return;

        var closeButton = modal.querySelector('[data-bs-dismiss="modal"], [data-dismiss="modal"], .modal-header .close');
        if (closeButton) {
            closeButton.click();
            return;
        }

        if (window.bootstrap && window.bootstrap.Modal) {
            var instance = window.bootstrap.Modal.getInstance(modal) || window.bootstrap.Modal.getOrCreateInstance(modal);
            instance.hide();
            return;
        }

        if (window.jQuery && window.jQuery.fn && window.jQuery.fn.modal) {
            window.jQuery(modal).modal('hide');
            return;
        }

        modal.classList.remove('show', 'in');
        modal.style.display = 'none';
        modal.setAttribute('aria-hidden', 'true');
        document.body.classList.remove('modal-open');
        document.querySelectorAll('.modal-backdrop').forEach(function (backdrop) {
            backdrop.parentNode.removeChild(backdrop);
        });
    }

    document.addEventListener('click', function (event) {
        var modal = visibleModal();
        if (!modal) return;

        var clickedModalBackground = event.target === modal;
        var clickedBackdrop = event.target.classList && event.target.classList.contains('modal-backdrop');

        if (clickedModalBackground || clickedBackdrop) {
            event.preventDefault();
            closeModal(modal);
        }
    }, true);
}());
