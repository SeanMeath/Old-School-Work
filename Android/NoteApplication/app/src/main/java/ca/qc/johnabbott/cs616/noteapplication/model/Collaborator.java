package ca.qc.johnabbott.cs616.noteapplication.model;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.annotations.Expose;

import ca.qc.johnabbott.cs616.noteapplication.sqlite.Identifiable;

public class Collaborator implements Identifiable<Long> {

    //region Fields
    @Expose
    private long id;
    @Expose
    private String note;
    @Expose
    private String user;

    private long noteId;
    private long userId;
    //endregion

    public Collaborator() {
    }

    @Override
    public Long getId() {
        return id;
    }

    @Override
    public void setId(Long id) {
        this.id = id;
    }

    public long getNoteId() {
        return noteId;
    }

    public Collaborator setNoteId(long noteId) {
        this.noteId = noteId;
        return this;
    }

    public long getUserId() {
        return userId;
    }

    public Collaborator setUserId(long userId) {
        this.userId = userId;
        return this;
    }

    public String getNote(){
        return note;
    }

    public Collaborator setNote(String noteUuid) {
        this.note = noteUuid;
        return this;
    }

    public String getUser(){
        return user;
    }

    public Collaborator setUser(String userUuid) {
        this.user = userUuid;
        return this;
    }

    public String format(){
        GsonBuilder builder = new GsonBuilder()
                .excludeFieldsWithoutExposeAnnotation();
        Gson gson = builder.create();
        String json = gson.toJson(this, Collaborator.class);

        return json;
    }
}

