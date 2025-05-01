<!DOCTYPE html>
<html>

<head>
        <link rel="stylesheet" type="text/css" href="style.css">
        <title>:) Categories</title>
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
        <h1>Categories</h1> 
        <?php
            require "Recipes_Connect.php";
            $conn = $GLOBALS["conn"];

            if($conn){
                $stid = oci_parse($conn, "BEGIN GetCategories(:bvCurs); END;");
                $curCatList = oci_new_cursor($conn);
                oci_bind_by_name($stid, ":bvCurs", $curCatList, -1, OCI_B_CURSOR);
                oci_execute($stid);
                oci_execute($curCatList);

                echo '<ul class="striped-list">';
                while(($row = oci_fetch_assoc($curCatList)) != false){
                    echo "<li><a href='Recipes_Category.php?category=" . $row['CATID'] . "&name=" . $row['CATNAME'] . "'>";
                    echo "<h1>" . $row['CATNAME'] . "</h1>";
                    echo "<p>" . $row['CATDESC'] . "</p>";
                    echo "</a></li>";
                }
                echo '</ul>';
            }
        ?>
        </div>
</body>

</html>