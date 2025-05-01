package ca.qc.johnabbott.cs406;

import java.io.FileReader;
import java.io.IOException;
import java.util.List;
import java.util.Scanner;

public class Main {

    public static void main(String[] args) {

        Trie test = new Trie();
//        test.add("hello");
//        test.add("my");
//        test.add("name");
//        test.add("is");
//        test.add("nano");
//        test.add("nam");
//        test.add("naa");
//        test.add("nab");
//        test.add("nac");
//        test.add("nad");
//
//        System.out.println(test.contains("hello"));
//        System.out.println(test.contains("my"));
//        System.out.println(test.contains("name"));
//        System.out.println(test.contains("is"));
//        System.out.println(test.contains("nano"));
//        System.out.println(test.contains("sean"));
//        System.out.println(test.contains("nam"));

        //Populate / Test using the Alphabet Class
        try{
            FileReader reader = new FileReader(Alphabets.LEXICON_ALPHABET);
            Scanner scanner = new Scanner(reader);

            String tmp;

            while(scanner.hasNextLine()){
                tmp = scanner.nextLine();
                test.add(tmp);
            }
        }
        catch (IOException e){
            System.out.println("Error");
        }

        List<String> sadFace = test.complete("na", 5);

        System.out.println(sadFace.toString());
    }
}
