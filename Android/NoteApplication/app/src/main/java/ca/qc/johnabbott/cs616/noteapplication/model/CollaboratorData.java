package ca.qc.johnabbott.cs616.noteapplication.model;
import java.util.ArrayList;
import java.util.List;

public class CollaboratorData {
    private static List<Collaborator> data;

    public static List<Collaborator> getData(){
        data = new ArrayList<>();
        return data;
    }
}