<!DOCTYPE html>
<html>

<head>
        <link rel="stylesheet" type="text/css" href="style.css">
        <title>:) Recipes</title>
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

                if(!isset($_REQUEST["ID"])){
                        $stid=oci_parse($conn, 'BEGIN :bvr := ChooseRandomRecipe(); END;');
                        oci_bind_by_name($stid, ':bvr', $recID, 32);
                        oci_execute($stid);
                }
                else{
                        $recID = $_REQUEST["ID"];
                }

                if(isset($_POST["RATING"])){
                        $stid = oci_parse($conn, "BEGIN AddReview(:bvId, :bvRating, :bvComments); END;");
                        oci_bind_by_name($stid, ":bvId", $recID);
                        oci_bind_by_name($stid, ":bvRating", $_POST["RATING"]);
                        oci_bind_by_name($stid, ":bvComments", $_POST["COMMENT"]);
                        oci_execute($stid);
                }

                // GET RECIPE
                $stid = oci_parse($conn, "BEGIN GetRecipe(:bvCurs, :bvRec); END;");
                $curRec = oci_new_cursor($conn);
                oci_bind_by_name($stid, ":bvCurs", $curRec, -1, OCI_B_CURSOR);
                oci_bind_by_name($stid, ":bvRec", $recID);
                oci_execute($stid);
                oci_execute($curRec);

                $rec = oci_fetch_assoc($curRec);
                
                // RECIPE INFO
                echo '<img src="Images/' . str_pad($rec['RECID'], 3, "0", STR_PAD_LEFT) . '.png" alt="ERROR">';
                echo "<h1>" . $rec['RECNAME'] . "</h1>";
                echo "<h2>" . "Description: " . $rec['RECDESC'] . "</h2>";
                echo '<br>';
                echo "<h2>" . "Instructions: " . $rec['INSTRUCTIONS'] . "</h2>";
                if($rec['USEFULTIP']){
                        echo "<h3>" . "Description: " . $rec['USEFULTIP'] . "</h3>";
                }
                echo "<h4>" . "PrepTime: " . $rec['PREPTIME'] . " Minutes</h4>";
                echo "<h4>" . "CookTime: " . $rec['COOKTIME'] . " Minutes</h4>";

                // NUTRITIONAL INFO
                $stid = oci_parse($conn, "BEGIN GetInfo(:bvCurs, :bvRec); END;");
                $curInf = oci_new_cursor($conn);
                oci_bind_by_name($stid, ":bvCurs", $curInf, -1, OCI_B_CURSOR);
                oci_bind_by_name($stid, ":bvRec", $recID);
                oci_execute($stid);
                oci_execute($curInf);
                if(($inf = oci_fetch_assoc($curInf)) != false){
                        echo '<h3>Nutritional Info (' . $inf['APPLIESTO'] . ')</h3>';
                        echo '<ul>';
                        echo '<li><h4>KCAL: ' . $inf['KCAL'] . '</h4></li>';
                        echo '<li><h4>FAT: ' . $inf['FAT'] . '</h4></li>';
                        echo '<li><h4>CARBS: ' . $inf['CARBS'] . '</h4></li>';
                        echo '<li><h4>SUGARS: ' . $inf['SUGARS'] . '</h4></li>';
                        echo '<li><h4>FIBRE: ' . $inf['FIBRE'] . '</h4></li>';
                        echo '<li><h4>PROTEIN: ' . $inf['PROTEIN'] . '</h4></li>';
                        echo '<li><h4>SALT: ' . $inf['SALT'] . '</h4></li>';
                        echo '</ul>';
                }

                // INGREDIENTS
                $stid = oci_parse($conn, "BEGIN GetIngredients(:bvCurs, :bvRec); END;");
                $curIng = oci_new_cursor($conn);
                oci_bind_by_name($stid, ":bvCurs", $curIng, -1, OCI_B_CURSOR);
                oci_bind_by_name($stid, ":bvRec", $recID);
                oci_execute($stid);
                oci_execute($curIng);
                echo '<h3>Ingredients</h3>';
                echo '<ul>';
                while(($ing = oci_fetch_assoc($curIng)) != false){

                        $stid = oci_parse($conn, "BEGIN GetAlternatives(:bvCurs, :bvIng); END;");
                        $curAlt = oci_new_cursor($conn);
                        oci_bind_by_name($stid, ":bvCurs", $curAlt, -1, OCI_B_CURSOR);
                        oci_bind_by_name($stid, ":bvIng", $ing['INGID']);
                        oci_execute($stid);
                        oci_execute($curAlt);

                        echo '<li><h4>' . $ing['INGNAME'] . ' ' . $ing['QUANTITY'] . ' ' . $ing['MEASURE'] . '</h4>';

                        while(($alt = oci_fetch_assoc($curAlt)) != false){
                                echo '<h6>(' . $alt['INGNAME'] . ')</h6>';
                        }

                        echo '</li>';


                }
                echo '</ul>';

                // TOOLS
                $stid = oci_parse($conn, "BEGIN GetTools(:bvCurs, :bvRec); END;");
                $curTool = oci_new_cursor($conn);
                oci_bind_by_name($stid, ":bvCurs", $curTool, -1, OCI_B_CURSOR);
                oci_bind_by_name($stid, ":bvRec", $recID);
                oci_execute($stid);
                oci_execute($curTool);
                echo '<h3>Tools</h3>';
                echo '<ul>';
                while(($tool = oci_fetch_assoc($curTool)) != false){
                        echo '<li><h4>' . $tool['TOOLNAME'] . '</h4></li>';
                }
                echo '</ul>';

                // RECOMMENDED
                $stid = oci_parse($conn, "BEGIN GetRecommended(:bvCurs, :bvRec); END;");
                $curReco = oci_new_cursor($conn);
                oci_bind_by_name($stid, ":bvCurs", $curReco, -1, OCI_B_CURSOR);
                oci_bind_by_name($stid, ":bvRec", $recID);
                oci_execute($stid);
                oci_execute($curReco);
                echo '<h4>Recommended</h4>';
                echo '<ul>';
                while(($reco = oci_fetch_assoc($curReco)) != false){
                        echo "<li><a href='Recipes_Recipe.php?ID=" . $reco['PAIRRECID'] . "'>";
                        echo "<h4>" . $reco['RECNAME'] . "</h4></a></li>";
                }
                echo '</ul>';

                $numStars = floor($rec['AVERAGERATING']);
                echo '<h4>Average Rating: ';
                for($i = 0; $i < $numStars; $i++){
                        echo '&#9733;';
                }
                echo '</h4>';
                echo '<h4>Total Reviews: ' . $rec['TOTALREVIEWS'] . '</h4>';

                // REVIEWS
                $stid = oci_parse($conn, "BEGIN GetReviews(:bvCurs, :bvRec); END;");
                $curRev = oci_new_cursor($conn);
                oci_bind_by_name($stid, ":bvCurs", $curRev, -1, OCI_B_CURSOR);
                oci_bind_by_name($stid, ":bvRec", $recID);
                oci_execute($stid);
                oci_execute($curRev);

                echo '<ul class="striped-list review">';
                while(($rev = oci_fetch_assoc($curRev)) != false){
                        echo '<li>';
                        echo '<h3>';
                        for($i = 0; $i < $rev['RATING']; $i++){
                                echo '&#9733;';
                        }
                        echo '</h3>';
                        echo '<h4>' . $rev['COMMENTS'] . '</h4>';
                        echo '</li>';
                }
                echo '</ul>';

                // REVIEW FORM
                echo '<form id="comment" action="Recipes_Recipe.php" method="POST">';
                echo '<h2>Add a Review to: ' . $rec['RECNAME'] . '</h2>';
                echo '<input type="hidden" name="ID" value=' . $recID . '>';
                echo '<ul>';
                echo '<li><h3>Comment:</h3><textarea name="COMMENT" rows="10" cols="30"></textarea></li>';
                echo '<li><h3>Rating: <select name="RATING">';
                echo '<option value=1>1</option>';
                echo '<option value=2>2</option>';
                echo '<option value=3>3</option>';
                echo '<option value=4>4</option>';
                echo '<option value=5>5</option>';
                echo '</select></h3></li>';
                echo '<li><input type="submit" value="Submit"></li>';
                echo '</ul></form>';
            }
        ?>
</body>

</html>
        </div>
</body>

</html>