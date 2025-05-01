<?php
require_once "db_cred.php";
require_once("smarty/libs/Smarty.class.php");

#Logged in
if(isset($_COOKIE["user"])){
    #Form Filed Out
    if(isset($_POST["email"])){

        $err="";
        $errorFlag = false;

        #region error checking
        if( strlen($_POST["fname"]) == 0){
            $errorFlag = true;
            $err.="First name Empty<br>";
        }
        if( strlen($_POST["lname"]) == 0){
            $errorFlag = true;
            $err.="Last Name Empty<br>";
        }
        if( strlen($_POST["user"]) == 0){
            $errorFlag = true;
            $err.="User Empty<br>";
        }
        if(strlen($_POST["email"]) == 0){
            $errorFlag = true;
            $err.="Email Empty<br>";
        }
        if(strlen($_POST["pass"]) == 0){
            $errorFlag = true;
            $err.="Password Empty<br>";
        }
        if(strlen($_POST["secQuestion"]) == 0){
            $errorFlag = true;
            $err.="Security Question Empty<br>";
        }
        if(strlen($_POST["secAnswer"]) == 0 ){
            $errorFlag = true;
            $err.="Security Answer Empty<br>";
        }
        if( $_POST["pass"] != $_POST["confirmPass"] ){
            $errorFlag = true;
            $err.="Passwords dont match<br>";
        }
        #endregion
        if(!$errorFlag){
            $dbConn = new mysqli(DB_HOST, DB_USER, DB_PASSWORD, DB_DATABASE);
            $sql="Select Count(Username) From Accounts Where Email != ? And Username = ?";
            $stmnt=$dbConn->prepare($sql);
            $stmnt->bind_param("ss", $_POST["email"], $_POST["user"]);
            $stmnt->bind_result($userCount);
            $stmnt->execute();
            $stmnt->fetch();
            $stmnt->close();

            if($userCount == 0){
                setcookie("user", time()-3600);
                setcookie("user", $_POST["user"], time()+3600);

                $sql="Update Accounts Inner Join Log On Accounts.Username = Log.Username
                      Set Accounts.Username=?, Log.Username=?, FirstName=?, LastName=?, Password=?, Security_Question=?, Security_Answer=?
                      Where Email=?";
                $stmnt=$dbConn->prepare($sql);
                $stmnt->bind_param("ssssssss", $_POST["user"], $_POST["user"], $_POST["fname"], $_POST["lname"], $_POST["pass"], $_POST["secQuestion"], $_POST["secAnswer"], $_POST["email"]);
                $stmnt->execute();
                $stmnt->close();

                $msg="Account Successfully Changed!<br>Welcome Back ".$_POST["fname"]." ".$_POST["lname"];
                $msg.="<br><br><a href='edit.php'>I wish to change my account</a>";
            }
            else{
                $errorFlag = true;
                $err.="Username already in use<br>";
            }
        }
        $smarty = new Smarty();
        $smarty->setTemplateDir("smarty/templates");
        $smarty->setCompileDir("smarty/templates_c");
        if($errorFlag){
            $smarty->assign("errors", $err);
            $smarty->display("error.html");
        }
        else{
            $smarty->assign("message", $msg);
            $smarty->display("success.html");
        }
    }
    else{
        $username = $_COOKIE["user"];
        $dbConn = new mysqli(DB_HOST, DB_USER, DB_PASSWORD, DB_DATABASE);
    
        $sql="Select FirstName, LastName, Email, Password, Security_Question, Security_Answer From Accounts Where Username = ?";
        $stmnt=$dbConn->prepare($sql);
        $stmnt->bind_param("s", $username);
        $stmnt->bind_result($firstName, $lastName, $email, $password, $question, $answer);
        $stmnt->execute();
        $stmnt->fetch();
        $stmnt->close();
    
        $smarty = new Smarty();
        $smarty->setTemplateDir("smarty/templates");
        $smarty->setCompileDir("smarty/templates_c");
    
        $smarty->assign("user", $username);
        $smarty->assign("fName", $firstName);
        $smarty->assign("lName", $lastName);
        $smarty->assign("email", $email);
        $smarty->assign("password", $password);
        $smarty->assign("question", $question);
        $smarty->assign("answer", $answer);
        $smarty->display("edit.html");
    }
}
else{
    header('Location: login.html');
}
?>