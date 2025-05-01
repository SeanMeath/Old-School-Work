package ca.qc.johnabbott.cs616.noteapplication.ui;

import android.app.Application;

public class NotesApplication extends Application {
    String ian = "7bdba0fe-fe95-4b1c-8247-f2479ee6e380";
    String usef = "3faa2495-f0f1-4408-ae24-d482f37caf1c";
    String aref = "6e840afc-5c0a-4679-bcfa-8a210e50ecfc";
    String jim = "97489bce-1c85-4ff2-b457-ba53589d12cc";
    String sandy = "2c77dafe-1545-432f-b5b1-3a0011cf7036";
    String nobody = "13cea3c0-4b18-471f-9bee-e9060ac62213";

    private String noteLink;
    private String notesLink;


    private String userUuid;
    private String connection;

    @Override
    public void onCreate() {
        super.onCreate();
        userUuid = ian;
        connection = "http://192.168.2.100:9999";
    }

    public String getUserUuid() {
        return userUuid;
    }

    public String getUserUrl(){
        return connection + "/user/" + userUuid;
    }

    public String getConnection() {
        return connection;
    }

    public String getUserNotesUrl(){
        return connection + "/user/" + userUuid + "/notes";
    }

    public String getUpdateUrl(String noteUuid){
        return connection + "/note/" + noteUuid;
    }

    public String getAddNoteUrl(){
        return connection + "/note";
    }

    public String getAddCollaboratorUrl(){
        return connection + "/collaborator";
    }
}