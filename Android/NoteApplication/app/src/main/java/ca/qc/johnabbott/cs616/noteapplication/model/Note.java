package ca.qc.johnabbott.cs616.noteapplication.model;
import android.os.Build;
import android.os.Parcel;
import android.os.Parcelable;
import androidx.annotation.RequiresApi;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.annotations.Expose;
import java.net.URL;
import java.util.Date;

import ca.qc.johnabbott.cs616.noteapplication.sqlite.Identifiable;

/**
 * Represent a single note in the "Notes" app.
 * @author Ian Clement (ian.clement@johnabbott.qc.ca)
 */
public class Note implements Identifiable<Long>, Parcelable {

    //region Internal Classes
    public class Notes{
        public class Embedded {
            public Note[] notes;
        }

        private Embedded _embedded;
    }

    public class Collabs{
        public class Embedded {
            public User[] collaborators;
        }

        private Embedded _embedded;
    }

    public class Links{
        public class Link{
            public URL href;
            public boolean templated;
        }

        private Link self;
        private Link note;
        private Link collaborators;
    }
    //endregion

    //Allows you to parcel the data
    public static final Parcelable.Creator CREATOR = new Creator<Note>() {
        @RequiresApi(api = Build.VERSION_CODES.Q)
        @Override
        public Note createFromParcel(Parcel src) {
            return new Note(src);
        }

        @Override
        public Note[] newArray(int size) {
            return new Note[size];
        }
    };

    //region Note Fields
    @Expose
    private String title;
    @Expose
    private String body;
    @Expose
    private Category category;
    @Expose
    private Date reminder;
    @Expose
    private Date created;
    @Expose
    private Date modified;

    private Long id;
    private String uuid;
    private Links _links;
    private Collabs _embeded;
    private boolean hasReminder;
    //endregion

    //region Constructors
    /**
     * Create a blank note.
     */
    public Note() {
        this(-1);
    }

    /**
     * Create a blank note with a specific ID.
     * @param id
     */
    public Note(long id) {
        this.id = id;
    }
    /**
     * Create a note.
     * @param id
     * @param title
     * @param body
     * @param category
     * @param hasReminder
     * @param reminder
     * @param created
     * @param modified
     */
    public Note(long id,  String body, String title,  boolean hasReminder, Date reminder, Category category,Date created, Date modified) {
        this.id = id;
        this.title = title;
        this.body = body;
        this.category = category;
        this.hasReminder = hasReminder;
        this.reminder = reminder;
        this.created = created;
        this.modified = modified;
    }

    //Read note from parcel
    @RequiresApi(api = Build.VERSION_CODES.Q)
    public Note(Parcel src) {
        id = src.readLong();
        title = src.readString();
        body = src.readString();
        category = Category.values()[src.readInt()];
        hasReminder = src.readBoolean();
        reminder = (Date) src.readSerializable();
        created = (Date) src.readSerializable();
        modified = new Date(src.readLong());
        uuid = src.readString();
    }
    //endregion

    //region Properties
    public Long getId() {
        return id;
    }

    @Override
    public void setId(Long id) {
        this.id = id;
    }

    public Note setId(long id) {
        this.id = id;
        return this;
    }

    public String getTitle() {
        return title;
    }

    public Note setTitle(String title) {
        this.title = title;
        return this;
    }

    public String getBody() {
        return body;
    }

    public Note setBody(String body) {
        this.body = body;
        return this;
    }

    public Category getCategory() {
        return category;
    }

    public Note setCategory(Category category) {
        this.category = category;
        return this;
    }

    public boolean isHasReminder() {
        return hasReminder;
    }

    public Note setHasReminder(boolean hasReminder) {
        this.hasReminder = hasReminder;
        return this;
    }

    public Date getReminder() {
        return reminder;
    }

    public Note setReminder(Date reminder) {
        this.reminder = reminder;
        return this;
    }

    public Date getCreated() {
        return created;
    }

    public Note setCreated(Date created) {
        this.created = created;
        return this;
    }

    public Date getModified() {
        return modified;
    }

    public Note setModified(Date modified) {
        this.modified = modified;
        return this;
    }

    public String getUuid() {
        return uuid;
    }

    public Note setUuid(String uuid) {
        this.uuid = uuid;
        return this;
    }

    public Links get_links() {
        return _links;
    }

    public void set_links(Links _links) {
        this._links = _links;
    }

    public Collabs get_embeded() {
        return _embeded;
    }

    public void set_embeded(Collabs _embeded) {
        this._embeded = _embeded;
    }
    //endregion

    @Override
    public int describeContents() {
        return 0;
    }

    /**
     * Create a duplicate (aka clone) of the note.
     * @return
     */
    public Note clone() {
        Note clone = new Note();
        clone.id = this.id;
        clone.title = this.title;
        clone.body = this.body;
        clone.category = this.category;
        clone.created = this.created;
        clone.hasReminder = this.hasReminder;
        clone.reminder = this.reminder;
        clone.modified = this.modified;
        return clone;
    }

    @Override
    public String toString() {
        return "Note{" +
                "id=" + id +
                ", title='" + title + '\'' +
                ", body='" + body + '\'' +
                ", category=" + category +
                ", hasReminder=" + hasReminder +
                ", reminder=" + reminder +
                ", created=" + created +
                ", modified=" + modified +
                '}';
    }

    //Write the contents to a parcel
    @RequiresApi(api = Build.VERSION_CODES.Q)
    @Override
    public void writeToParcel(Parcel dest, int flags) {
        dest.writeLong(id);
        dest.writeString(title);
        dest.writeString(body);
        dest.writeInt(category.ordinal());
        dest.writeBoolean(hasReminder);
        dest.writeSerializable(reminder);
        dest.writeSerializable(created);
        dest.writeLong(modified.getTime());
        dest.writeString(uuid);
    }

    public static Note parse(String json){
        //System.out.println(json);
        GsonBuilder builder = new GsonBuilder()
                .setDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSSZ");
        Gson gson = builder.create();
        Note note = gson.fromJson(json, Note.class);

        if (note.reminder != null)
        {
            note.hasReminder = true;
        }

        String href = note._links.self.href.toString();
        String id = href.split("/")[4];
        note.setUuid(id);

        return note;
    }

    public static Note[] parseArray(String json){
        //System.out.println(json);
        GsonBuilder builder = new GsonBuilder()
                .setDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSSZ");
        Gson gson = builder.create();
        Notes notes = gson.fromJson(json, Notes.class);

        for (Note note : notes._embedded.notes){
            if (note.reminder != null)
            {
                note.hasReminder = true;
            }

            String href = note._links.self.href.toString();
            String id = href.split("/")[4];
            note.setUuid(id);
        }

        return notes._embedded.notes;
    }

    public String format(){
        GsonBuilder builder = new GsonBuilder()
                .excludeFieldsWithoutExposeAnnotation()
                .setDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSSZ");
        Gson gson = builder.create();
        String json = gson.toJson(this, Note.class);
        return json;
    }
}