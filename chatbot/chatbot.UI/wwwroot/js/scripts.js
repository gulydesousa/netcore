function toggleButtonDisabled() {
    var input = document.getElementsByName("message")[0];
    var button = document.getElementById("submitButton");
    button.disabled = input.value.trim() === "";
}

function DisplayProgressMessage(form, msg) {
    var button = form.querySelector('button[type="submit"]');
    button.disabled = true;
    $(".spinner-border").removeClass("d-none");
    return true;
}