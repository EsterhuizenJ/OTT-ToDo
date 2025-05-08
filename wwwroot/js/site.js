// Show modal function
function showModal(modalId) {
    var myModal = new bootstrap.Modal(document.getElementById(modalId));
    myModal.show();
}

// Hide modal function
function hideModal(modalId) {
    var myModal = bootstrap.Modal.getInstance(document.getElementById(modalId));
    if (myModal) {
        myModal.hide();
    }
}