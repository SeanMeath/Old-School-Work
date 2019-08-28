Create or Replace Function ChooseRandomRecipe
Return Number
As
rID Number;
Begin
    Select RecID Into rID From
    (Select * From Recipes Order By DBMS_RANDOM.VALUE)
    Fetch First Rows Only;
    Return rID;
End;

/

Create or Replace Procedure GetRecipe
(
    curRecipe OUT sys_refcursor,
    rID IN VARCHAR2
)
AS
BEGIN
    OPEN curRecipe FOR
    Select * From Recipes Where RecID = rID;
END;

/

Create or Replace Procedure GetCategories
(curCategories OUT sys_refcursor)
AS
BEGIN
    OPEN curCategories FOR
    Select CatID, CatName, CatDesc From Categories Order By CatName;
END;

/

Create or Replace Procedure GetCategory
(
    curRecipes OUT sys_refcursor,
    cID IN VARCHAR2
)
AS
BEGIN
    OPEN curRecipes FOR
    Select RecID, RecName, RecDesc, Serves, CookTime, PrepTime, AverageRating From Recipes Natural Join RecipesInCategories Where CatID = cID;
END;

/

Create Table Reviews 
( 
    ReviewID Number(10,0) Generated Always as Identity, 
    RecID Number(10,0), 
    Rating Number(1,0), 
    Comments Varchar2(200), 
    Constraint Rev_RevID_pk Primary Key (ReviewID), 
    Constraint Rev_RecID_fk Foreign Key (RecID) References Recipes(RecID), 
    Constraint Rev_Rating_ck Check ((Rating) Between 0 And 5) 
)

/

Create or Replace Trigger Reviews_B_I
BEFORE Insert ON Reviews
FOR EACH ROW
DECLARE
    SumRating Number;
    NumRating Number;
    AvgRating Number;
BEGIN
    Select Count(Rating) As NumOfEntries Into NumRating From Reviews Where RecID = :new.RecID;
    IF NumRating = 0 Then
        SumRating:=0;
    Else
        Select Count(Rating) As NumOfEntries Into NumRating From Reviews Where RecID = :new.RecID Group By RecID;
        Select Sum(Rating) As SumOfRatings Into SumRating From Reviews Where RecID = :new.RecID Group By RecID;
    End If;
    NumRating := NumRating + 1;
    SumRating := SumRating + :new.Rating;
    AvgRating := SumRating / NumRating;
    Update Recipes Set AverageRating = AvgRating, TotalReviews = NumRating Where RecID = :new.RecID;
END;

/

Create or Replace Procedure AddReview
(
    rID IN VARCHAR2,
    rating IN NUMBER,
    comment IN VARCHAR2
)
AS
BEGIN
    Insert Into Ratings (RECID, RATING, COMMENTS) Values (rID, rating, comment);
    commit;
END;

/

Create or Replace Procedure GetReviews
    (
    curReviews OUT sys_refcursor,
    rID IN VARCHAR2
    )
AS
BEGIN
    OPEN curReviews FOR
    Select Rating, Comments From Reviews Where RecID = rID Order By DBMS_RANDOM.VALUE
    Fetch First 3 Rows Only;
END;

/

Create or Replace View vwFiveIngredientsOrLess
As
Select RecID, RecName, Count(IngID) As Number_of_Ingredients
From Recipes Natural Join IngredientsInRecipes
Group By RecID, RecName
Having Count(IngID) <= 5
Order By Number_of_Ingredients;

/

Create or Replace Procedure GetEasy
(
    curRecipes OUT sys_refcursor
)
AS
BEGIN
    OPEN curRecipes FOR
    Select RecID, RecName, RecDesc, Serves, CookTime, PrepTime, AverageRating From Recipes
    Where RecID In (Select RecID From vwFiveIngredientsOrLess);
END;

/

Create or Replace View vwOneHourOrLess
As
Select RecID, RecName, (PrepTime + CookTime) As Total_Time
From Recipes
Where (PrepTime + CookTime) <= 60
Order By Total_Time;

/

Create or Replace Procedure GetQuick
(
    curRecipes OUT sys_refcursor
)
AS
BEGIN
    OPEN curRecipes FOR
    Select RecID, RecName, RecDesc, Serves, CookTime, PrepTime, AverageRating From Recipes
    Where RecID In (Select RecID From vwOneHourOrLess);
END;

/

Create or Replace Procedure Search
(
    curRecipes OUT sys_refcursor,
    searchTerm IN VARCHAR2
)
AS
BEGIN
    OPEN curRecipes FOR
    Select RecID, RecName, RecDesc, Serves, CookTime, PrepTime, AverageRating From Recipes Natural Join IngredientsInRecipes Natural Join Ingredients
    Where Upper(IngName) Like '%' || Upper(searchTerm) || '%' Group By RecID, RecName, RecDesc, Serves, CookTime, PrepTime, AverageRating;
END;

/

Create or Replace Procedure GetIngredients
    (
    curIngs OUT sys_refcursor,
    rID IN VARCHAR2
    )
AS
BEGIN
    OPEN curIngs FOR
    Select IngName, Quantity, Measure, IngID From Ingredients Natural Join IngredientsInRecipes Where RecID = rID;
END;

/

Create or Replace Procedure GetInfo
    (
    curInf OUT sys_refcursor,
    rID IN VARCHAR2
    )
AS
BEGIN
    OPEN curInf FOR
    Select KCAL, FAT, CARBS, SUGARS, FIBRE, PROTEIN, SALT, APPLIESTO From NutritionalInfo Where RecID = rID;
END;

/

Create or Replace Procedure GetTools
    (
    curInf OUT sys_refcursor,
    rID IN VARCHAR2
    )
AS
BEGIN
    OPEN curInf FOR
    Select TOOLNAME From ToolsInRecipe Natural Join Tools Where RecID = rID;
END;

/

Create or Replace Procedure GetRecommended
    (
    curInf OUT sys_refcursor,
    rID IN VARCHAR2
    )
AS
BEGIN
    OPEN curInf FOR
    Select PairsWith.PairRecID, Recipes.RecName
    From PairsWith Inner Join Recipes
    On PairsWith.PairRecID = Recipes.RecID
    Where PairsWith.RecID = rID;
END;

/

Create or Replace Procedure GetAlternatives
    (
    curIng OUT sys_refcursor,
    iID IN VARCHAR2
    )
AS
BEGIN
    OPEN curIng FOR
    Select IngName
    From AlternativeIngredients Inner Join Ingredients
    On AlternativeIngredients.AltIngID = Ingredients.IngID
    Where AlternativeIngredients.IngID = iID;
END;

/

Create or Replace Function CtoF
(
    Cur Number
)
Return Number
As
    Cal Number;
Begin
    Cal:=Cur*9;
    Cal:=Cal/5;
    Cal:=Cal+32;
    Cal:=ROUND(Cal);
    return Cal;
END;

/

Create or Replace Function FtoC
(
    Cur Number
)
Return Number
As
    Cal Number;
Begin
    Cal:=Cur-32;
    Cal:=Cal*5;
    Cal:=Cal/9;
    Cal:=ROUND(Cal);
    Return Cal;
END;

/

Create or Replace Function ConvertOvenTemp
(
    CurrentTemp Number,
    CurrentFormat VARCHAR2,
    DesiredFormat VARCHAR2
)
Return Number
As
    CalculatedTemp Number;
Begin
    Case
        When CurrentFormat = 'C' AND DesiredFormat = 'F' Then
            CalculatedTemp:=CtoF(CurrentTemp);
        When CurrentFormat = 'F' AND DesiredFormat = 'C' Then
            CalculatedTemp:=FtoC(CurrentTemp);
        When CurrentFormat = DesiredFormat Then
            CalculatedTemp:=CurrentTemp;
        When CurrentFormat = 'G' Then
            CalculatedTemp:=CurrentTemp*14;
            CalculatedTemp:=CalculatedTemp+121;
            IF DesiredFormat = 'F' Then
                CalculatedTemp:=CtoF(CalculatedTemp);
            END IF;
        Else
            IF CurrentFormat = 'F' Then
                CalculatedTemp:=FtoC(CurrentTemp);
            Else
                CalculatedTemp:=CurrentTemp;
            END IF;
            CalculatedTemp:=CalculatedTemp-121;
            CalculatedTemp:=CalculatedTemp/14;
            CalculatedTemp:=Round(CalculatedTemp, 1);
    End Case;
    Return CalculatedTemp;
End;