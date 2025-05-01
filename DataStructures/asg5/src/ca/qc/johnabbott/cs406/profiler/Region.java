package ca.qc.johnabbott.cs406.profiler;

public class Region {
    private long time;
    private int count;
    private String label;
    private double percent;

    //Creates and initialoses the variables of the region
    public Region(String label_, long time_){
        time = time_;
        label = label_;
        count = 1;
    }

    //Changes the percent as the time divided by the time of the section it dwells in
    public void setPercent(double total_){
        percent = time / total_;
    }

    //Adds the last run time to the total runtime and increase count
    //Called when another instance of this region is called
    public void modifyRegion(Long time_){
        time += time_;
        count++;
    }

    //get total time elapsed so far in region
    public long getElapsedTime() {
        return time;
    }

    //get percent of the time of the section
    public double getPercentOfSection() {
        return percent;
    }

    //Gets how often this region was called/created
    public int getRunCount() {
        return count;
    }

}
