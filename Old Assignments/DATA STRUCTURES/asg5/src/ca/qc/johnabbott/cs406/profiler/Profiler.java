package ca.qc.johnabbott.cs406.profiler;

import java.util.*;

/**
 * A simple profiling class.
 *
 * @author TODO
 */
public class Profiler {

    private static Profiler EXISTENCE;

    // singleton
    public static Profiler getInstance(){
        if(EXISTENCE == null)
            EXISTENCE = new Profiler();
        return  EXISTENCE;
    }

    /*
       Delimits a section or region of the profiling.
     */
    private static class Mark {

        // stores the type of mark
        public enum Type {
            START_REGION, END_REGION, START_SECTION, END_SECTION
        }

        public Type type;
        public long time;
        public String label;

        // Create mark without a label
        public Mark(Type type, long time) {
            this(type, time, null);
        }

        // Create a mark with a label
        public Mark(Type type, long time, String label) {
            this.type = type;
            this.time = time;
            this.label = label;
        }

        @Override
        public String toString() {
            return "Mark{" +
                    "type=" + type +
                    ", time=" + time +
                    ", label='" + label + '\'' +
                    '}';
        }
    }

    // store marks in list
    private List<Mark> marks;

    // use to prevent regions when not wanted/needed.
    private boolean inSection;


    // private constructor for singleton
    private Profiler() {
        // linked list, because append is a constant time operation.
        marks = new LinkedList<>();
        inSection = false;
    }

    /**
     * Starts a new profiling section.
     * @param label The section label.
     */
    public void startSection(String label) {
        marks.add(new Mark(Mark.Type.START_SECTION, System.nanoTime(), label));
        inSection = true;
    }

    /**
     * Ends a section. Must be paired with a corresponding call to `startSection(..)`.
     */
    public void endSection() {
        marks.add(new Mark(Mark.Type.END_SECTION, System.nanoTime()));
        inSection = false;
    }

    /**
     * Starts a new profiling region.
     * @param label The region label.
     */
    public void startRegion(String label) {
        if(inSection)
            marks.add(new Mark(Mark.Type.START_REGION, System.nanoTime(), label));
    }

    /**
     * Ends a region. Must be paire with a corresponding call to `startRegion(..)`.
     */
    public void endRegion() {
        if(inSection)
            marks.add(new Mark(Mark.Type.END_REGION, System.nanoTime()));
    }

    //creates sections and regions based on the marks created throughout the runtime of the profiler
    public List<Section> getSections() {
        //must be done all sections
        if(inSection){throw new IllegalArgumentException();}

        //holds all the sections
        List<Section> sections = new ArrayList<>();
        //holds the start marks to keep track of the timestamps and the labels
        List<Mark> starts = new LinkedList<>();
        //holds the regions in the section in progress of populating
        Map<String, Region> section = new HashMap<>();
        //hold the current start of the mark (to compare to the end)
        Mark start;
        //holds the current region to modify if you run into already created region of a section
        Region currentRegion;

        //loop through each mark
        for(Mark mark : marks){
            //if its a start then add it to the list of marks
            if(mark.type==Mark.Type.START_SECTION) {
                starts.add(mark);
            }
            //if its the end of a section then get the start of the section and create a new section into the list and reset the section
            else if(mark.type==Mark.Type.END_SECTION){
                //get start mark
                start = starts.remove(starts.size()-1);
                //create a new section into the list
                sections.add(new Section(start.label, mark.time-start.time, section));
                //reset the section
                section = new HashMap<>();
            }
            //if its a start then add it to the list of marks
            else if(mark.type==Mark.Type.START_REGION){
                starts.add(mark);
            }
            //if its the end of a region then get the start of the region and create region if it doesn't exist. Otherwise update it
            else if (mark.type==Mark.Type.END_REGION){
                //get start mark
                start = starts.remove(starts.size()-1);
                //if it already exists
                if (section.containsKey(start.label)) {
                    //get the old instance of the region
                    currentRegion = section.get(start.label);
                    //update the region
                    currentRegion.modifyRegion(mark.time-start.time);
                }
                //does not exist
                else {
                    //create a new region into the map
                    section.put(start.label, new Region(start.label, mark.time-start.time));
                }
            }
        }
        return sections;
    }

    @Override
    public String toString() {
        StringBuilder builder = new StringBuilder();
        for(Mark mark : marks)
            builder.append(String.format("%d %13s %-30s\n", mark.time, mark.type.toString(), mark.label != null ? mark.label : ""));
        return builder.toString();
    }
  
}
