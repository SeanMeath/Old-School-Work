<!DOCTYPE html>
<html>

<head>
        <link rel="stylesheet" type="text/css" href="style.css">
        <title>:) Category</title>
</head>

<body>
        <nav>
        <ul id="topNav">
                <li><a class="active" href="Recipes_HomePage.php">:) Recipes</a></li>
                <li><a href="Recipes_Categories.php">Categories</a></li>
                <li><a href="Recipes_Recipe.php">Random Recipe</a></li>
                <li style="float:right">
                    <form action="Recipes_Search.php" method="GET">
                            <input type="text" id="searchBar" name="Search" placeholder="Search...">
                            <input type="submit" id="searchButton" value="Search">
                    </form>
                </li>
        </ul>
        </nav>

        <div id="mainPage">
        <?php
        require "Recipes_Connect.php";
        $conn = $GLOBALS["conn"];

        if($conn){
            $cat = $_GET["category"];
            $nam = $_GET["name"];
            echo "<h1>Recipes in Category: $nam</h1>";

            $stid = oci_parse($conn, "BEGIN GetCategory(:bvCurs, :bvCat); END;");
            $curRecList = oci_new_cursor($conn);
            oci_bind_by_name($stid, ":bvCurs", $curRecList, -1, OCI_B_CURSOR);
            oci_bind_by_name($stid, ":bvCat", $cat);
            oci_execute($stid);
            oci_execute($curRecList);

            echo '<ul class="striped-list">';
            while(($row = oci_fetch_assoc($curRecList)) != false){
                echo "<li class='recipeTile'><a href='Recipes_Recipe.php?ID=" . $row['RECID'] . "'>";
                echo '<img src="Images/' . str_pad($row['RECID'], 3, "0", STR_PAD_LEFT) . '.png" alt="ERROR">';
                echo "<h1>" . $row['RECNAME'] . "</h1>";
                echo "<p>" . "Description: " . $row['RECDESC'] . "</p>";
                echo "<p>" . "PrepTime: " . $row['PREPTIME'] . " Minutes</p>";
                echo "<p>" . "CookTime: " . $row['COOKTIME'] . " Minutes</p>";

                $numStars = floor($row['AVERAGERATING']);
                echo '<p>Average Rating: ';
                for($i = 0; $i < $numStars; $i++){
                        echo '&#9733;';
                }
                echo '</p>';

                echo "</a></li>";
            }
            echo '</ul>';
        }
        ?>
        </div>
</body>

</html>