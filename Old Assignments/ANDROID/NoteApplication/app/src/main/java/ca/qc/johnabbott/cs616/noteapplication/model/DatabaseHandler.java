package ca.qc.johnabbott.cs616.noteapplication.model;
import android.content.Context;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.os.Build;

import androidx.annotation.Nullable;
import androidx.annotation.RequiresApi;

import ca.qc.johnabbott.cs616.noteapplication.sqlite.Table;
import ca.qc.johnabbott.cs616.noteapplication.sqlite.TableFactory;

public class DatabaseHandler extends SQLiteOpenHelper {

    //GLOBALS FOR DATABASE METADATA AND RECORDS
    //region GLOBALS
    public static final String DATABASE_NAME = "notes.db";
    public static final int DATABASE_VERSION = 2;
    private final Table<Note> note;
    private final Table<User> user;
    private final Table<Collaborator> collaborator;
    //endregion

    //Constructor to create the note table off of the data retrieved
    public DatabaseHandler(@Nullable Context context) {
        super(context, DATABASE_NAME, null, DATABASE_VERSION);
        note = TableFactory.makeFactory(this, Note.class)
                .setSeedData(NoteData.getData())
                .useDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSSZ")
                .getTable();

        user = TableFactory.makeFactory(this, User.class)
                .setSeedData(UserData.getData(context))
                .getTable();

        collaborator = TableFactory.makeFactory(this, Collaborator.class)
                .setSeedData(CollaboratorData.getData())
                .getTable();
    }

    //Get the table from the database
    public Table<Note> getNoteTable(){
        return note;
    }

    //Get the table from the database
    public Table<User> getUserTable(){
        return user;
    }

    //Get the table from the database
    public Table<Collaborator> getCollaboratorTable(){
        return collaborator;
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        //Create the table
        db.execSQL(note.getCreateTableStatement());
        db.execSQL(user.getCreateTableStatement());
        db.execSQL(collaborator.getCreateTableStatement());

        //If the tables has data
        if(note.hasInitialData()){
            //Set the data
            note.initialize(db);
        }
        if(user.hasInitialData()){
            //Set the data
            user.initialize(db);
        }
        if(collaborator.hasInitialData()){
            //Set the data
            collaborator.initialize(db);
        }

    }

    //Upgrade drops and recreates the database
    @Override
    public void onUpgrade(SQLiteDatabase db, int i, int i1) {
        db.execSQL(note.getDropTableStatement());
        db.execSQL(note.getCreateTableStatement());
    }
}