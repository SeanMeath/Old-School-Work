<?php
require_once "db_cred.php";
require_once("smarty/libs/Smarty.class.php");

$dbConn = new mysqli(DB_HOST, DB_USER, DB_PASSWORD, DB_DATABASE);

$hasAdded = false;

if ( $_POST["searchFor_1"] == "" ) {
    $sql = "select username, email, firstName, lastName from accounts";
    $stmnt = $dbConn->prepare($sql);
    $stmnt->bind_result($username, $email, $firstName, $lastName);
    $stmnt->execute();
} else {
    $sql = "select username, email, firstName, lastName from accounts
            where ".$_POST["searchUsing_1"]." like ?";

    if( isset($_POST["exactMatch_1"]) )
        $searchFor_1 = $_POST['searchFor_1'];
    else
        $searchFor_1 = "%".$_POST["searchFor_1"]."%";

    if($_POST["operator"] != "done"){
        if ( $_POST["searchFor_2"] != "" ) {
            $sql .= " ".$_POST["operator"]." ".$_POST["searchUsing_2"]." like ?";
            if( isset($_POST["exactMatch_2"]) )
                $searchFor_2 = $_POST['searchFor_2'];
            else
                $searchFor_2 = "%".$_POST["searchFor_2"]."%";

            $stmnt = $dbConn->prepare($sql);
            $stmnt->bind_param("ss", $searchFor_1, $searchFor_2);
            $stmnt->bind_result($username, $email, $firstName, $lastName);
            $stmnt->execute();

            $hasAdded = true;
        }
    }

    if(!$hasAdded){
        $stmnt = $dbConn->prepare($sql);
        $stmnt->bind_param("s", $searchFor_1);
        $stmnt->bind_result($username, $email, $firstName, $lastName);
        $stmnt->execute();
    }
}

$theRows = Array();
$counter = 0;
while ($stmnt->fetch()) {
    $theRow["username"] = $username;
    $theRow["email"] = $email;
    $theRow["firstName"] = $firstName;
    $theRow["lastName"] = $lastName;
    $theRow["counter"] = $counter;
    $counter++;
    $theRows[]=$theRow;
}

$stmnt->close();
$dbConn->close();

$smarty = new Smarty();
$smarty->setTemplateDir("smarty/templates");
$smarty->setCompileDir("smarty/templates_c");
$smarty->assign("trows", $theRows);
$smarty->assign("counter", $counter);
$smarty->display("admin_search_results.html");
?>