window.addEventListener("load", PageIsLoaded);

function PageIsLoaded(){
    var theUsername = document.getElementById("user");
    var theEmail = document.getElementById("email");
    var theFirstName = document.getElementById("fname");
    var theLastName = document.getElementById("lname");
    var thePassword = document.getElementById("pass");
    var theConfirmedPassword = document.getElementById("confirmPass");
    var theSecurityQuestion = document.getElementById("secQuestion");
    var theSecurityAnswer = document.getElementById("secAnswer");

    theUsername.addEventListener("keyup", adjustStars);
    theUsername.addEventListener("focusout", adjustStars);
    theEmail.addEventListener("keyup", adjustStars);
    theEmail.addEventListener("focusout", adjustStars);
    theFirstName.addEventListener("keyup", adjustStars);
    theFirstName.addEventListener("focusout", adjustStars);
    theLastName.addEventListener("keyup", adjustStars);
    theLastName.addEventListener("focusout", adjustStars);
    thePassword.addEventListener("keyup", adjustStars);
    thePassword.addEventListener("focusout", adjustStars);
    theConfirmedPassword.addEventListener("keyup", adjustStars);
    theConfirmedPassword.addEventListener("focusout", adjustStars);
    theSecurityQuestion.addEventListener("keyup", adjustStars);
    theSecurityQuestion.addEventListener("focusout", adjustStars);
    theSecurityAnswer.addEventListener("keyup", adjustStars);
    theSecurityAnswer.addEventListener("focusout", adjustStars);

    thePassword.addEventListener("focusout", checkDictionary);
}

function adjustLoginButton(){
    var loginButton = document.getElementById("submit");
   
    loginButton.disabled = true;

    if(checkValidity()){
        loginButton.disabled = false;
    }
    checkUser();
}

function adjustStars(event){
    adjustLoginButton();
    
    var star;
    switch(event.target.id){
        case("user"):
            star = document.getElementById("star1");
            break;
        case("email"):
            star = document.getElementById("star2");
            break;
        case("pass"):
            star = document.getElementById("star3");
            break;
        case("confirmPass"):
            star = document.getElementById("star4");
            break;
        case("secQuestion"):
            star = document.getElementById("star5");
            break;
        case("secAnswer"):
            star = document.getElementById("star6");
            break;
        case("fname"):
            star = document.getElementById("starA");
            break;
        case("lname"):
            star = document.getElementById("starB");
            break;
    }
    theTarget=document.getElementById(event.target.id);
    if(theTarget.value.length==0){
        star.style.display="inline";
    }
    else{
        star.style.display="none";
    }
}

function checkUser(){
    var username = document.getElementById("user").value;

    var data = {};
    data.user = username;
    $.post("checkUser.php", data, process_user_return);
}

function checkDictionary(){
    var word = document.getElementById("pass").value;

    var data = {};
    data.word = word;
    $.post("checkDictionary.php", data, process_word_return);
}

function process_user_return ($data) {
    message = document.getElementById("alert");
    var loginButton = document.getElementById("submit");
    if($data == "true"){
        loginButton.disabled = true;
        message.textContent="Username already in use :(";
    }
    else{
        message.textContent="";
    }
}

function process_word_return ($data) {
    message = document.getElementById("alert2");
    var loginButton = document.getElementById("submit");
    var thePassword = document.getElementById("pass");
    var theConfirmedPassword = document.getElementById("confirmPass");
    if($data == "true"){
        message.textContent="Password consists of valid words or their inverse :(";
        thePassword.value="";
        theConfirmedPassword.value="";

        thePassword.focus();

    }
    else{
        message.textContent="";
    }
}

function checkValidity(){
    var theUsername = document.getElementById("user");
    var theEmail = document.getElementById("email");
    var theFirstName = document.getElementById("fname");
    var theLastName = document.getElementById("lname");
    var thePassword = document.getElementById("pass");
    var theConfirmedPassword = document.getElementById("confirmPass");
    var theSecurityQuestion = document.getElementById("secQuestion");
    var theSecurityAnswer = document.getElementById("secAnswer");

    if ( theUsername.value.length != 0
         && theEmail.value.length != 0
         && theFirstName.value.length != 0
         && theLastName.value.length != 0
         && thePassword.value.length != 0
         && theConfirmedPassword.value.length != 0
         && theSecurityQuestion.value.length != 0
         && theSecurityAnswer.value.length != 0) {
        if(thePassword.value == theConfirmedPassword.value){
            return true;
        }
    }
    return false;
}