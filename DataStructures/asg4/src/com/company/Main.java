package com.company;
import java.util.Scanner;

public class Main {

    private static class DoubleLink<T> {
        public T element;
        public DoubleLink<T> next;
        public DoubleLink<T> prev;
        public DoubleLink() {}
        public DoubleLink(T element) {
            this.element = element;
            this.next = this;
            this.prev = this;
        }
    }

    public static void main(String[] args) {

        //Set Initial Direction
        String order = "clockwise";

        //Get the variables
        int numberOfElements = readNumber("Please enter the number of elements you wish to have in the circle", "Error! The answer provided does not consist of a valid positive number. Please try again.");
        int clockwiseJump = readNumber("Please enter the clockwise jump", "Error! The answer provided does not consist of a valid positive number. Please try again.");
        int counterClockwiseJump = readNumber("Please enter the counter-clockwise jump", "Error! The answer provided does not consist of a valid positive number. Please try again.");

        //Output the Variables Chosen
        System.out.println();
        System.out.println("NumberOfElements: " + numberOfElements);
        System.out.println("clockwiseJump: " + clockwiseJump);
        System.out.println("counterClockwiseJump: " + counterClockwiseJump);
        System.out.println();
        System.out.println("Order Removed: ");

        //Create the circle
        DoubleLink<Integer> dbLink = createDoubleLink(numberOfElements);

        //While there is more then one thing in the circle
        while(dbLink.next != dbLink){
            //If the direction is clockwise
            if(order == "clockwise"){
                //Remove the element corresponding to the i'th position going clockwise
                dbLink = removePartOfDoubleLink(dbLink, order, clockwiseJump);
                //Swap the order
                order = "counterClockwise";
            }
            //If the direction is counter-clockwise
            else {
                //Remove the element corresponding to the i'th position going counter-clockwise
                dbLink = removePartOfDoubleLink(dbLink, order, counterClockwiseJump);
                //Swap the order
                order = "clockwise";
            }
        }

        //Print the last element
        System.out.println(dbLink.element);
        //Empty the double link
        dbLink = null;
        //Prove that it's empty
        System.out.println(dbLink);

    }

    public static DoubleLink<Integer> createDoubleLink(int numOfElements){
        //Create a DoubleLink
        DoubleLink<Integer> loopedLink = new DoubleLink<>();
        //Create a Dummy DoubleLink
        DoubleLink<Integer> progressor = loopedLink;

        //Loops until you reach the number of elements
        //Creating a new link in each instance of the loop
        for(int i = 1; i < numOfElements; i++){
            //Set the element of the dummy DoubleLink to i
            progressor.element = i;
            //Create a temporary DoubleLink
            DoubleLink<Integer> tmp = new DoubleLink<>();
            //Set the dummy's next to the temporary DoubleLink
            progressor.next = tmp;
            //Set the temporary's previous to the dummy DoubleLink
            tmp.prev = progressor;
            //set the dummy to the temporary
            progressor = progressor.next;
        }
        //Set the last element
        progressor.element = numOfElements;
        //Set the last element's next to the start
        progressor.next = loopedLink;
        //Set the first element's previous to the end
        loopedLink.prev = progressor;

        return loopedLink;
    }

    public static DoubleLink<Integer> removePartOfDoubleLink(DoubleLink<Integer> doubleLink, String direction, int jump) {
        //If no jump then return the DoubleLink with no changes
        if(jump == 0) return doubleLink;
        else{
            //Move to the i'th element
            for (int i = 1; i < jump; i++){
                //Step forward or backwards depending on the direction
                if (direction == "clockwise") doubleLink = doubleLink.next;
                else doubleLink = doubleLink.prev;
            }
            //Remove the references to the element to remove
            doubleLink.next.prev = doubleLink.prev;
            doubleLink.prev.next = doubleLink.next;

            //Print the element removed
            System.out.print(doubleLink.element + " ");
            //Step forward to lose all ties to it
            doubleLink = doubleLink.next;
            return  doubleLink;
        }
    }

    public static int readNumber(String demandForInput, String errorMessage){
        //Initialase the variables
        Scanner scan;
        Boolean success = false;
        int number = 0;
        //Loop until done
        while(!success){
            try{
                //Ask for the demand
                System.out.println(demandForInput);
                //Receive the input
                scan = new Scanner(System.in);
                //Attempt to send the input to int
                number = scan.nextInt();
                //We succeed
                success = true;
                //Attempt to clear console :(
                System.out.print("\033[H\033[2J");
                System.out.flush();
            }
            catch(java.util.InputMismatchException e){
                //Attempt to clear console :(
                System.out.print("\033[H\033[2J");
                System.out.flush();
                //Output error
                System.out.println(errorMessage);
            }
        }
        return number;
    }
}