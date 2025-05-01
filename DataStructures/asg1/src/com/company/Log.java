package com.company;

import java.sql.Timestamp;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.time.format.DateTimeFormatter;
import java.util.Date;

/**
 * A log entry for the logfile in Asg #1
 * @author YOU!
 */
public class Log implements Comparable<Log> {

    // Fields
    private Date date;
    private IPAddress ipAddress;
    private String serviceName;
    private int length;

    // Constructors
    public Log(String readText) throws ParseException {
        String[] logInfo = readText.split("\\s+");
        ipAddress = new IPAddress(logInfo[0]);
        serviceName = logInfo[1];
        SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS");
        date = format.parse(logInfo[2]);
        length = Integer.parseInt(logInfo[3]);
    }

    // Getters and setters
    // TODO
    public Date getDate() {
        return date;
    }

    public void setDate(Date timestamp) {
        this.date = timestamp;
    }

    public IPAddress getIpAddress() {
        return ipAddress;
    }

    public void setIpAddress(IPAddress ipAddress) {
        this.ipAddress = ipAddress;
    }

    public String getServiceName() {
        return serviceName;
    }

    public void setServiceName(String serviceName) {
        this.serviceName = serviceName;
    }

    public int getLength() {
        return length;
    }

    public void setLength(int length) {
        this.length = length;
    }

    @Override
    public int compareTo(Log rhs) {
        // TODO
        int ipCompare = this.ipAddress.compareTo(rhs.getIpAddress());
        if (ipCompare != 0) {
            return ipCompare;
        }
        int serviceNameCompare = this.serviceName.compareTo(rhs.getServiceName());
        if (serviceNameCompare != 0) {
            return -serviceNameCompare;
        }
        int dateCompare = this.date.compareTo(rhs.getDate());
        if (dateCompare != 0) {
            return dateCompare;
        }
        return 0;
    }

    @Override
    public String toString() {
        SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS");
        String output = (ipAddress + "\t" + serviceName + " \t" + format.format(date) + "\t" + length);
        return output;
    }
}
