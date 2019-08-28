package ca.qc.johnabbott.cs406.serialization.util;

import java.io.IOException;
import java.nio.ByteBuffer;
import java.util.Date;
import java.util.Objects;

import ca.qc.johnabbott.cs406.serialization.Serializable;
import ca.qc.johnabbott.cs406.serialization.SerializationException;
import ca.qc.johnabbott.cs406.serialization.Serializer;
import ca.qc.johnabbott.cs406.serialization.util.SDate;

/**
 * Represents a grade.
 *
 * @author Ian Clement (ian.clement@johnabbott.qc.ca)
 * @since 2018-04-29
 */
public class Grade implements Serializable {

    private String name;
    private int result;
    private Date date;

    public static final byte SERIAL_ID = 0x05;

    public Grade(String name, int result, Date date) {
        this.name = name;
        this.result = result;
        this.date = date;
    }

    public Grade() {
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getResult() {
        return result;
    }

    public void setResult(int result) {
        this.result = result;
    }

    public Date getDate() {
        return date;
    }

    public void setDate(Date date) {
        this.date = date;
    }

    @Override
    public String toString() {
        return "Grade{" +
                "name='" + name + '\'' +
                ", result=" + result +
                ", date=" + date +
                '}';
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Grade grade = (Grade) o;
        return result == grade.result &&
                Objects.equals(name, grade.name) &&
                Objects.equals(date, grade.date);
    }

    @Override
    public int hashCode() {
        return Objects.hash(name, result, date);
    }

    @Override
    public byte getSerialId() {
        return SERIAL_ID;
    }

    @Override
    public void writeTo(Serializer s) throws IOException {
        //Set up buffer for the string
        byte[] bytes = new byte[name.length() + Integer.BYTES];
        ByteBuffer buffer = ByteBuffer.wrap(bytes);
        //Enter the length of the string to the buffer
        buffer.putInt(name.length());
        //Add each char of the string to the buffer
        for(char c : name.toCharArray())
            buffer.put((byte)c);
        //Write the buffer to the file
        s.write(buffer.array());
        //Clear the buffer
        buffer.clear();

        //Set up buffer for the integer
        bytes = new byte[Integer.BYTES];
        buffer = ByteBuffer.wrap(bytes);
        //Enter the integer to the buffer
        buffer.putInt(result);
        //Write the buffer to the file
        s.write(buffer.array());
        //Clear the buffer
        buffer.clear();

        //Set up buffer for the long
        bytes = new byte[Long.BYTES];
        buffer = ByteBuffer.wrap(bytes);
        //Enter the long to the buffer
        buffer.putLong(date.getTime());
        //Write the buffer to the file
        s.write(buffer.array());
    }

    @Override
    public void readFrom(Serializer s) throws IOException, SerializationException {
        //Read string from bin file
        byte[] bytes = s.readNext();
        name = new String(bytes);

        //Set up buffer for the integer
        bytes = new byte[Integer.BYTES];
        ByteBuffer buffer = ByteBuffer.wrap(bytes);
        bytes = buffer.array();
        //Read integer from bin file
        s.read(bytes);
        result = buffer.getInt();
        //Clear the buffer
        buffer.clear();

        //Set up buffer for the long
        bytes = new byte[Long.BYTES];
        buffer = ByteBuffer.wrap(bytes);
        bytes = buffer.array();
        //Read integer from bin file
        s.read(bytes);
        Long tmpDate = buffer.getLong();
        //Convert to date
        date = new Date(tmpDate);
    }

    @Override
    public boolean immutable() {
        return false;
    }
}
