package ca.qc.johnabbott.cs406.profiler;

import ca.qc.johnabbott.cs406.profiler.Region;
import ca.qc.johnabbott.cs406.collections.list.ArrayList;

import java.util.HashMap;
import java.util.Map;

public class Section {
    public static String TOTAL="TOTAL";
    private String label;
    private Long time;
    private Map<String, Region> regions;

    //creates a section from a hashmap of regions
    public Section(String label_, Long time_, Map<String, Region> regions_){
        this.label = label_;
        this.time = time_;
        this.regions = regions_;
        //creates a region for the total time in section
        this.regions.put(TOTAL, new Region(TOTAL, time));
        for(String regionLabel : this.regions.keySet()) {
            //label.setPercent(time);
            Region tmp = this.regions.get(regionLabel);
            //setting the regions percent of the section
            tmp.setPercent(this.time);
        }
    }

    //returns all the regions in the section
    public Map<String, Region> getRegions() {
        return regions;
    }

    //returns the label of the section
    public String getLabel() {
        return label;
    }
}