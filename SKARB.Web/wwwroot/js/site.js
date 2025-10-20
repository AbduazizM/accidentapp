
function generatePassword(passwordFieldId, confirmFieldId) {
    const lower = "abcdefghijklmnopqrstuvwxyz";
    const upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const digits = "0123456789";
    const symbols = "!#$%^&*?";

    let password =
        lower.charAt(Math.floor(Math.random() * lower.length)) +
        upper.charAt(Math.floor(Math.random() * upper.length)) +
        digits.charAt(Math.floor(Math.random() * digits.length)) +
        symbols.charAt(Math.floor(Math.random() * symbols.length));

    const all = lower + upper + digits + symbols;
    for (let i = 0; i < 8; i++) {
        password += all.charAt(Math.floor(Math.random() * all.length));
    }

    password = password.split('').sort(() => 0.5 - Math.random()).join('');

    const passwordField = document.getElementById(passwordFieldId);
    const confirmField = document.getElementById(confirmFieldId);

    if (passwordField) passwordField.value = password;
    if (confirmField) confirmField.value = password;

    if (passwordField) passwordField.classList.remove("is-invalid");
    if (confirmField) confirmField.classList.remove("is-invalid");
}