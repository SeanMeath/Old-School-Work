window.addEventListener("load", PageIsLoaded);

function PageIsLoaded(){
    var submitButton = document.getElementById("submit");

    submitButton.addEventListener("click", checkForErrors);
}

function checkForErrors(event){
    var theFirstName = document.getElementById("fname");
    var theLastName = document.getElementById("lname");
    var theUsername = document.getElementById("user");
    var theEmail = document.getElementById("email");
    var thePassword = document.getElementById("pass");
    var theConfirmedPassword = document.getElementById("confirmPass");
    var theSecurityQuestion = document.getElementById("secQuestion");
    var theSecurityAnswer = document.getElementById("secAnswer");

    var errorFlag = false;
    var err = "";

    if ( theFirstName.value.length == 0){
        err += "First Name Can't Be Empty\n";
        errorFlag = true;
    }
    if ( theLastName.value.length == 0){
        err += "Last Name Can't Be Empty\n";
        errorFlag = true;
    }
    if ( theUsername.value.length == 0){
        err += "Username Can't Be Empty\n";
        errorFlag = true;
    }
    if ( theEmail.value.length == 0){
        err += "Email Can't Be Empty\n";
        errorFlag = true;
    }
    else if(!theEmail.value.includes(".") || !theEmail.value.includes("@")){
        err += "Password Must Contain Both A Capitalized And A Now Capitalized Character\n";
        errorFlag = true;
    }
    if ( thePassword.value.length == 0){
        err += "Password Can't Be Empty\n";
        errorFlag = true;
        
    }
    else if(thePassword.value.length < 6){
        err += "Password Must Be Larger Than 6 Characters\n";
        errorFlag = true;
    }
    else if(!(/[A-Z]/.test(thePassword.value)) || !(/[a-z]/.test(thePassword.value))){
        err += "Password Must Contain Both A Capitalized And A Now Capitalized Character\n";
        errorFlag = true;
    }
    else if ( thePassword.value != theConfirmedPassword.value){
        err += "The Password And The Confrim Password Must Match\n";
        errorFlag = true;
        
    }
    if ( theSecurityQuestion.value.length == 0){
        err += "Security Question Can't Be Empty\n";
        errorFlag = true;
        
    }
    if ( theSecurityAnswer.value.length == 0){
        err += "Security Answer Can't Be Empty\n";
        errorFlag = true;
        
    }

    if(errorFlag){
        alert(err);
        event.preventDefault();
    }
}