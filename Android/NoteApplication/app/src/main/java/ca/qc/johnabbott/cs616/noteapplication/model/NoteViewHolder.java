package ca.qc.johnabbott.cs616.noteapplication.model;
import android.content.res.Resources;
import android.graphics.Color;
import android.view.MotionEvent;
import android.view.View;
import android.widget.TextView;
import androidx.annotation.NonNull;
import androidx.constraintlayout.widget.ConstraintLayout;
import androidx.recyclerview.widget.RecyclerView;
import java.text.SimpleDateFormat;
import ca.qc.johnabbott.cs616.noteapplication.R;

public class NoteViewHolder extends RecyclerView.ViewHolder implements View.OnTouchListener {

    //GLOBALS FOR COMPONENTS
    //region GLOBALS
    private final View root;
    private final ConstraintLayout noteLayout;
    private final TextView titleTextView;
    private final TextView bodyTextView;
    private final TextView reminderTextView;
    private long id;
    private Integer position;
    OnNoteClickListener mainActivity;
    Boolean hasMoved;
    //endregion

    //Constructor
    public NoteViewHolder(@NonNull final View root, OnNoteClickListener mainActivity) {
        super(root);
        //Grab the views to be used
        noteLayout = root.findViewById(R.id.note_ConstraintLayout);
        titleTextView = root.findViewById(R.id.title_TextView);
        bodyTextView = root.findViewById(R.id.body_TextView);
        reminderTextView = root.findViewById(R.id.reminder_TextView);
        this.root = root;

        //Holds the main activity to call a function
        this.mainActivity = mainActivity;

        //Set the holder (tile/note) ontouch
        root.setOnTouchListener(this);
    }

    //Set the specific holder to a given note
    public void set(Note note, int position){
        //Get the note information for identification
        this.position = position;
        this.id = note.getId();

        //Set the note string data
        titleTextView.setText(note.getTitle());
        bodyTextView.setText(note.getBody());

        //Base date format used
        SimpleDateFormat fmt = new SimpleDateFormat("yyyy-MM-dd HH:mm");

        //If it has a reminder set the reminder date and time
        if(note.isHasReminder()){
            reminderTextView.setText(fmt.format(note.getReminder()));
        }
        else{
            reminderTextView.setText("");
        }

        //Get the color resources
        Resources res = root.getResources();

        //Set the background color to reflect the category chosen
        switch(note.getCategory()){
            case RED:
                noteLayout.setBackgroundColor(res.getColor(R.color.base08));
                break;
            case ORANGE:
                noteLayout.setBackgroundColor(res.getColor(R.color.base09));
                break;
            case YELLOW:
                noteLayout.setBackgroundColor(res.getColor(R.color.base0A));
                break;
            case GREEN:
                noteLayout.setBackgroundColor(res.getColor(R.color.base0B));
                break;
            case LIGHT_BLUE:
                noteLayout.setBackgroundColor(res.getColor(R.color.base0C));
                break;
            case DARK_BLUE:
                noteLayout.setBackgroundColor(res.getColor(R.color.base0D));
                break;
            case PURPLE:
                noteLayout.setBackgroundColor(res.getColor(R.color.base0E));
                break;
            case BROWN:
                noteLayout.setBackgroundColor(res.getColor(R.color.base0F));
                break;
            case NONE:
                noteLayout.setBackgroundColor(Color.TRANSPARENT);
                break;
        }
    }

    //On touch of the holder (tile/note)
    @Override
    public boolean onTouch(View view, MotionEvent motionEvent) {
        switch (motionEvent.getAction()) {
            //If the finger is pressed set the boolean of movement to false
            case MotionEvent.ACTION_DOWN:
                hasMoved = false;
                break;
            //Once movement has occurred set the boolean to true
            case MotionEvent.ACTION_MOVE:
                hasMoved = true;
                break;
            //Once the finger is released you check the movement boolean
            //If the movement is false then you initiate the method from the fragment to appear the menu
            case MotionEvent.ACTION_UP:
                if (!hasMoved) {
                    //Pass in the note information for identification and the touch coordinates
                    mainActivity.onClickNote(id, position, motionEvent.getRawX(), motionEvent.getRawY());
                }
                break;
        }
        return true;
    }
}
