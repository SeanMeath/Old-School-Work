package ca.qc.johnabbott.cs406;

import ca.qc.johnabbott.cs406.collections.list.LinkedList;
import ca.qc.johnabbott.cs406.collections.map.HashMap;
import ca.qc.johnabbott.cs406.collections.set.TreeSet;
import ca.qc.johnabbott.cs406.serialization.io.BufferedChannel;
import ca.qc.johnabbott.cs406.serialization.Serializer;
import ca.qc.johnabbott.cs406.serialization.util.*;

import java.io.IOException;
import java.io.RandomAccessFile;
import java.util.Date;

/**
 * Serialization example.
 */
public class MainSerialize {

    public static void main(String arg[]) throws IOException {

        BufferedChannel channel = new BufferedChannel(new RandomAccessFile("foo.bin", "rw").getChannel(), BufferedChannel.Mode.WRITE);

        Serializer serializer = new Serializer(null, channel);

        SInteger i = new SInteger(123);
        SString s = new SString("hello,");
        Box<SString> bs = new Box<>(new SString("world."));
        SDate d = new SDate();
        Grade g = new Grade("Josh", 91, new Date());
        LinkedList l = new LinkedList();
        for (int j = 0; j < 10; j++) {
            l.add(new SInteger(j));
        }
        HashMap<SInteger, SInteger> hs = new HashMap<>();
        for (int j = 0; j < 10; j++) {
            hs.put(new SInteger(j), new SInteger(j*2));
        }
        TreeSet<SInteger> ts = new TreeSet<>();
        for (int j = 0; j < 10; j++) {
            ts.add(new SInteger(j));
            ts.add(new SInteger(-j));
        }

        serializer.serialize(i);
        serializer.serializeNull();
        serializer.serialize(s);
        serializer.serializeNull();
        serializer.serialize(bs);
        serializer.serializeNull();
        serializer.serialize(d);
        serializer.serializeNull();
        serializer.serialize(g);
        serializer.serializeNull();
        serializer.serialize(l);
        serializer.serializeNull();
        serializer.serialize(hs);
        serializer.serializeNull();
        serializer.serialize(ts);
        serializer.serializeNull();

        channel.close();

    }
}
