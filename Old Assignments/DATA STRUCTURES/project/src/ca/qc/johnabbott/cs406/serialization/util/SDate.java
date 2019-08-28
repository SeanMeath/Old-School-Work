package ca.qc.johnabbott.cs406.serialization.util;

import ca.qc.johnabbott.cs406.serialization.Serializable;
import ca.qc.johnabbott.cs406.serialization.SerializationException;
import ca.qc.johnabbott.cs406.serialization.Serializer;

import java.io.IOException;
import java.nio.ByteBuffer;
import java.util.Date;
import java.util.Objects;

/**
 * SInteger class
 *
 * - Serializable wrapper class for the Integer class.
 *
 * Format:
 *
 * 1.  Write integer as 4 bytes.
 *
 */
public class SDate implements Serializable, Comparable<SDate> {

    // optimization, use a static ByteBuffer to avoid extra allocations on each (de)serialize operation.
    private static final ByteBuffer buffer;

    // ininitalize a static field in a static initialization block.
    static {
        byte[] bytes = new byte[Long.BYTES];
        buffer = ByteBuffer.wrap(bytes);
    }

    // store the wrapped integer value.
    private long dateVal;

    /**
     * Serializable 0 value.
     */
    public SDate() {
        dateVal = new Date().getTime();
    }

    /**
     * Custom serializable value.
     * @param value
     */
    public SDate(long value) {
        this.dateVal = value;
    }

    /**
     * Get the value of the serializable integer.
     * @return
     */
    public Date get() {
        Date stringDate = new Date(dateVal);
        return stringDate;
    }

    @Override
    public String toString() {
        Date stringDate = new Date(dateVal);
        return String.valueOf(stringDate);
    }

    @Override
    public int compareTo(SDate rhs) {
        return (int)(rhs.dateVal - this.dateVal);
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        SDate SDate = (SDate) o;
        return dateVal == SDate.dateVal;
    }

    @Override
    public int hashCode() {
        return Objects.hash(dateVal);
    }

    public static final byte SERIAL_ID = 0x04;

    @Override
    public byte getSerialId() {
        return SERIAL_ID;
    }

    @Override
    public void writeTo(Serializer s) throws IOException {
        // clear the buffer in case of previous use.
        buffer.clear();

        // place the integer
        buffer.putLong(dateVal);
        s.write(buffer.array());
    }

    @Override
    public void readFrom(Serializer s) throws IOException, SerializationException {

        // clear the buffer in case of previous use.
        buffer.clear();

        // extract the backing byte[] and read into it.
        byte[] bytes = buffer.array();
        s.read(bytes);

        // read the integer value from the ByteBuffer,
        // since the backing byte[] now has the data.
        dateVal = buffer.getLong();
    }

    @Override
    public boolean immutable() {
        return true;
    }
}