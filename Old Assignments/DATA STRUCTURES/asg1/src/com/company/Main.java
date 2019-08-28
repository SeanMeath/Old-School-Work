package com.company;

import java.io.*;
import java.util.Scanner;

public class Main {

    //GLOBAL COUNTER
    public static int currentFile;

    //GOES THROUGH EACH FILE AND MERGES THEM
    public static void main(String[] args) throws FileNotFoundException {
        for(currentFile = 1; currentFile <= 8; currentFile++){
            merge("data/test" + currentFile + "/in1.txt", "data/test" + currentFile + "/in2.txt", "data/test" + currentFile + "/out.txt");
        }
    }

    //MERGES THE FILES `in1` AND `in2` INTO `out`, MAINTAINING THE SORTED ORDER.
    public static void merge(String in1, String in2, String out) {
        try {
            //INITIALISATION OF VARIABLES
            Log log1 = null;
            Log log2 = null;
            Integer compare = 0;
            FileReader reader1 = new FileReader(in1);
            Scanner scanner1 = new Scanner(reader1);
            FileReader reader2 = new FileReader(in2);
            Scanner scanner2 = new Scanner(reader2);
            PrintWriter writer = new PrintWriter(out);

            //CHECKS IF THE FILES ARE EMPTY (END OF FILE)
            Boolean eof1 = !scanner1.hasNextLine();
            Boolean eof2 = !scanner2.hasNextLine();

            //LOOPS UNTIL ONE FILE ENDS
            while (!eof1 && !eof2) {
                //BOTH WERE EQUAL OR START OF LOOP
                if (compare == 0) {
                    log1 = new Log(scanner1.nextLine());
                    log2 = new Log(scanner2.nextLine());
                }
                //ALREADY PRINTED CURRENT LOG1 (GET NEW ONE)
                else if (compare > 0) {
                    log1 = new Log(scanner1.nextLine());
                }
                //ALREADY PRINTED CURRENT LOG2 (GET NEW ONE)
                else {
                    log2 = new Log(scanner2.nextLine());
                }

                //COMPARE TO SEE WHAT ONE TO WRITE
                compare = log1.compareTo(log2);

                //LOG1 HAS PRIORITY
                if (compare > 0) {
                    writer.println(log1.toString());
                }
                //LOG2 HAS PRIORITY
                else if (compare < 0) {
                    writer.println(log2.toString());
                }
                //LOG1 AND LOG2 HAVE EQUAL PRIORITY
                else {
                    writer.println(log1.toString());
                    writer.println(log2.toString());
                }

                //CHECKS IF THE FILES ARE EMPTY (END OF FILE)
                eof1 = !scanner1.hasNextLine();
                eof2 = !scanner2.hasNextLine();
            }

            //EMPTY THE NON USED LOG
            if (compare > 0) {
                writer.println(log2.toString());
            } else if (compare < 0) {
                writer.println(log1.toString());
            }

            //GOES THROUGH FILE 1 (FILE 2 ALREADY EMPTY)
            while (!eof1) {
                log1 = new Log(scanner1.nextLine());
                writer.println(log1.toString());

                //CHECKS IF THE FILES ARE EMPTY (END OF FILE)
                eof1 = !scanner1.hasNextLine();
            }
            //GOES THROUGH FILE 2 (FILE 1 ALREADY EMPTY)
            while (!eof2) {
                log2 = new Log(scanner2.nextLine());
                writer.println(log2.toString());

                //CHECKS IF THE FILES ARE EMPTY (END OF FILE)
                eof2 = !scanner2.hasNextLine();
            }

            //FINALISE THE CLOSERS + SCANNERS
            writer.close();
            scanner1.close();
            scanner2.close();

            //CONFIRM END
            System.out.println("Finished");

        } catch (java.io.IOException e) {
            System.err.println("Failed at File #" + currentFile);
            System.err.println(e.getMessage());
        } catch (java.text.ParseException e) {
            System.err.println("Failed at File #" + currentFile);
            System.err.println(e.getMessage());
        } catch (java.lang.RuntimeException e) {
            System.err.println("Failed at File #" + currentFile);
            System.err.println(e.getMessage());
        }
    }
}