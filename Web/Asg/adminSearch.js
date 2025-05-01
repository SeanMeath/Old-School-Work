window.addEventListener("load", PageIsLoaded);



function PageIsLoaded(){
    var operator = document.getElementById("operator");
    var searchButton = document.getElementById("search");

    $("#searchFor_20").hide();
    $("#searchUsing_2").hide();
    $("#exactMatch_2").hide();

    operator.addEventListener("change", adjustDisplay); 
    searchButton.addEventListener("click", validateSearch); 
}

function adjustDisplay(){
    var operator = document.getElementById("operator").value;

    if(operator == "done"){
        $("#searchFor_20").fadeOut("slow");
        $("#searchUsing_2").fadeOut("slow");
        $("#exactMatch_2").fadeOut("slow");
    }
    else{
        $("#searchFor_20").fadeIn("slow");
        $("#searchUsing_2").fadeIn("slow");
        $("#exactMatch_2").fadeIn("slow");
    }
}

function validateSearch(event){
    var searchField_1 = document.getElementById("searchFor_1");
    var searchField_2= document.getElementById("searchFor_2");
    var operator = document.getElementById("operator").value;

    var errorFlag = false;
    var err = "";

    if ( searchField_1.value.length == 0 ) {
        searchField_1.focus();
        errorFlag = true;
        err += "Search Field 1 Empty :( \n";
    }
    if( operator != "done" && searchField_2.value.length == 0 ){
        if(!errorFlag){
            searchField_2.focus();
        }
        errorFlag = true;
        err += "Search Field 2 Empty :( \n";
    }

    if(errorFlag){
        alert(err);
        event.preventDefault();
    }
}