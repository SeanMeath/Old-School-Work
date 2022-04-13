window.addEventListener("load", PageIsLoaded);

function PageIsLoaded(){
    var theUsername = document.getElementById("user");
    var thePassword = document.getElementById("pass");

    theUsername.addEventListener("focusout", adjustStars);
    thePassword.addEventListener("focusout", adjustStars);  
    
    theUsername.addEventListener("keyup", adjustLoginButton);
    thePassword.addEventListener("keyup", adjustLoginButton);  
}

function adjustLoginButton(){
    var theUsername = document.getElementById("user");
    var thePassword = document.getElementById("pass");
    var loginButton = document.getElementById("submit");
   
    loginButton.disabled = true;

    if ( theUsername.value.length != 0 && thePassword.value.length != 0) {
        loginButton.disabled = false;
    }
}

function adjustStars(event){
    var star;
    switch(event.target.id){
        case("user"):
            star = document.getElementById("star1");
            break;
        case("pass"):
            star = document.getElementById("star2");
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