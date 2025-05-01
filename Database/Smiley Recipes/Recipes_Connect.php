<html>
 	<head>
  		<title>PHP IS BAD AND MAKES ME SAD</title>
 	</head>
    <body> 	
    <?php
        $username = 'user1753546';
        $password = 'password';
        $dbname = '//10.39.167.152/pdborcl';

        $conn = oci_connect($username, $password, $dbname) or die;
    ?>
    </body>
</html>