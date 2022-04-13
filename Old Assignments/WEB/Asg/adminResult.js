window.addEventListener("load", PageIsLoaded);

function PageIsLoaded(){
    var deleteButton = document.getElementById("delete");

    var selectAllButton = document.getElementById("select_all");
    var clearAllButton = document.getElementById("clear_all");
    var selectOthersButton = document.getElementById("select_others");

    selectAllButton.addEventListener("click", selectAll); 
    clearAllButton.addEventListener("click", clearAll); 
    selectOthersButton.addEventListener("click", selectOthers); 

    deleteButton.addEventListener("click", checkDelete); 
}

function selectAll(){
    var form = document.forms[0];

    for(i = 0; i < form.length; i++){
        if (form[i].type == "checkbox")
            form[i].checked = true;
    }
}

function clearAll(){
    var form = document.forms[0];

    for(i = 0; i < form.length; i++){
        if (form[i].type == "checkbox")
            form[i].checked = false;
    }
}

function selectOthers(){
    var form = document.forms[0];

    for(i = 0; i < form.length; i++){
        if (form[i].type == "checkbox"){
            if (form[i].checked)
                form[i].checked = false;
            else
                form[i].checked = true;
        }   
    }
}

function checkDelete(event){
    var form = document.forms[0];

    var found = false;
    var toDel = "";

    for(i = 0; i < form.length; i++){
        if (form[i].type == "checkbox")
            if (form[i].checked){
                if(found){
                    toDel += ", ";
                }
                toDel += form[i].value;
                found = true;
            }
    }

    if(!found){
        alert("No things to delete :(");
        event.preventDefault();
    }
    else{
        if (!confirm("About to delete: " + toDel + "?")) {
            event.preventDefault();
        }
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