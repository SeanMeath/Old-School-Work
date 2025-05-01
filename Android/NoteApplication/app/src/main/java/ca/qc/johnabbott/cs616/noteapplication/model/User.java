package ca.qc.johnabbott.cs616.noteapplication.model;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.util.Base64;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonDeserializationContext;
import com.google.gson.JsonDeserializer;
import com.google.gson.JsonElement;
import com.google.gson.JsonParseException;
import com.google.gson.JsonPrimitive;
import com.google.gson.JsonSerializationContext;
import com.google.gson.JsonSerializer;
import java.io.ByteArrayOutputStream;
import java.lang.reflect.Type;
import java.net.URL;
import java.util.Objects;
import ca.qc.johnabbott.cs616.noteapplication.sqlite.Identifiable;

public class User implements Identifiable<Long> {

    //region Internal Classes
    public class Users{
        public class Embedded{
            public User[] users;
        }

        private Embedded _embedded;
    }

    public class Notes{
        public class Embedded {
            public Note[] notes;
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

    private static class BitmapToBase64TypeAdapter implements JsonSerializer<Bitmap>, JsonDeserializer<Bitmap> {
        public Bitmap deserialize(JsonElement json, Type typeOfT, JsonDeserializationContext context) throws JsonParseException {
            byte[] bytes = Base64.decode(json.getAsString(), Base64.NO_WRAP);
            return BitmapFactory.decodeByteArray(bytes, 0, bytes.length);
        }

        public JsonElement serialize(Bitmap src, Type typeOfSrc, JsonSerializationContext context) {
            ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
            src.compress(Bitmap.CompressFormat.PNG, 100, outputStream);
            return new JsonPrimitive(Base64.encodeToString(outputStream.toByteArray(), Base64.NO_WRAP));
        }
    }
    //endregion

    //region Fields
    private String name;
    private String email;
    private Bitmap avatar;
    private String password;
    private Notes _embeded;
    private Links _links;
    private long id;
    private String uuid;
    //endregion

    //region Constructors
    public User() {
        this(-1);
    }

    public User(long id) {
        this.id = id;
    }

    public User(long id, String name, Bitmap avatar, String email) {
        this(id);
        this.name = name;
        this.avatar = avatar;
        this.email = email;
    }
    //endregion

    //region Properties
    @Override
    public Long getId() {
        return id;
    }

    @Override
    public void setId(Long id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public User setName(String name) {
        this.name = name;
        return this;
    }

    public Bitmap getAvatar() {
        return avatar;
    }

    public User setAvatar(Bitmap avatar) {
        this.avatar = avatar;
        return this;
    }

    public String getEmail() {
        return email;
    }

    public User setEmail(String email) {
        this.email = email;
        return this;
    }

    public String getUuid() {
        return uuid;
    }

    public User setUuid(String uuid) {
        this.uuid = uuid;
        return this;
    }
    //endregion

    @Override
    public String toString() {
        return "User{" +
                "id=" + id +
                ", name='" + name + '\'' +
                ", avatar=" + avatar +
                ", email='" + email + '\'' +
                '}';
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        User user = (User) o;
        return id == user.id;/* &&
                Objects.equals(name, user.name) &&
                Objects.equals(avatar, user.avatar) &&
                Objects.equals(email, user.email);*/
    }

    @Override
    public int hashCode() {
        return Objects.hash(id, name, avatar, email);
    }

    public static User parse(String json){
        //System.out.println(json);
        GsonBuilder builder = new GsonBuilder()
                .setDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSSZ")
                .registerTypeHierarchyAdapter(Bitmap.class, new BitmapToBase64TypeAdapter());
        Gson gson = builder.create();
        User user = gson.fromJson(json, User.class);

        String href = user._links.self.href.toString();
        String id = href.split("/")[4];
        user.setUuid(id);

        return user;
    }

    public static User[] parseArray(String json){
        //System.out.println(json);
        GsonBuilder builder = new GsonBuilder()
                .setDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSSZ")
                .registerTypeHierarchyAdapter(Bitmap.class, new BitmapToBase64TypeAdapter());
        Gson gson = builder.create();
        Users users = gson.fromJson(json, Users.class);

        for (User user : users._embedded.users){
            String href = user._links.self.href.toString();
            String id = href.split("/")[4];
            user.setUuid(id);
        }

        return users._embedded.users;
    }
}