package ca.qc.johnabbott.cs406;

import ca.qc.johnabbott.cs406.collections.list.LinkedList;
import ca.qc.johnabbott.cs406.collections.map.HashMap;
import ca.qc.johnabbott.cs406.collections.set.TreeSet;
import ca.qc.johnabbott.cs406.serialization.io.BufferedChannel;
import ca.qc.johnabbott.cs406.serialization.SerializationException;
import ca.qc.johnabbott.cs406.serialization.Serializer;
import ca.qc.johnabbott.cs406.serialization.util.*;

import java.io.IOException;
import java.io.RandomAccessFile;

/**
 * Deserialize example.
 */
public class MainDeserialize {
    public static void main(String arg[]) throws IOException, SerializationException {

        Serializer serializer = new Serializer(
                new BufferedChannel(
                        new RandomAccessFile("foo.bin", "rw").getChannel()
                        , BufferedChannel.Mode.READ
                ), null);

        SInteger i = (SInteger) serializer.deserialize();
        System.out.println(serializer.deserialize());
        SString s = (SString) serializer.deserialize();
        System.out.println(serializer.deserialize());
        Box<SString> bs = (Box<SString>) serializer.deserialize();
        System.out.println(serializer.deserialize());
        SDate d = (SDate) serializer.deserialize();
        System.out.println(serializer.deserialize());
        Grade g = (Grade) serializer.deserialize();
        System.out.println(serializer.deserialize());
        LinkedList<SInteger> l = (LinkedList<SInteger>)  serializer.deserialize();
        System.out.println(serializer.deserialize());
        HashMap<SInteger, SInteger> hs = (HashMap<SInteger, SInteger>)  serializer.deserialize();
        System.out.println(serializer.deserialize());
        TreeSet<SInteger> ts = (TreeSet<SInteger>)  serializer.deserialize();
        System.out.println(serializer.deserialize());

        System.out.println(i);
        System.out.println(s);
        System.out.println(bs);
        System.out.println(d);
        System.out.println(g);
        System.out.println(l);
        System.out.println(hs);
        System.out.println(ts);

        serializer.close();

    }
}
