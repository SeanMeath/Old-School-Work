package ca.qc.johnabbott.cs616.noteapplication.ui.editor;
import androidx.annotation.RequiresApi;
import androidx.constraintlayout.widget.ConstraintLayout;
import androidx.fragment.app.Fragment;
import android.app.DatePickerDialog;
import android.app.TimePickerDialog;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.graphics.drawable.Drawable;
import android.os.Build;
import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CompoundButton;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.Switch;
import android.widget.TextView;
import android.widget.TimePicker;
import java.text.SimpleDateFormat;
import java.util.ArrayDeque;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.Stack;
import ca.qc.johnabbott.cs616.noteapplication.R;
import ca.qc.johnabbott.cs616.noteapplication.model.Category;
import ca.qc.johnabbott.cs616.noteapplication.model.Collaborator;
import ca.qc.johnabbott.cs616.noteapplication.model.DatabaseHandler;
import ca.qc.johnabbott.cs616.noteapplication.model.Note;
import ca.qc.johnabbott.cs616.noteapplication.model.OnCollaboratorAdded;
import ca.qc.johnabbott.cs616.noteapplication.model.User;
import ca.qc.johnabbott.cs616.noteapplication.model.UserData;
import ca.qc.johnabbott.cs616.noteapplication.sqlite.DatabaseException;
import ca.qc.johnabbott.cs616.noteapplication.ui.util.AddCollaboratorDialogFragment;
import ca.qc.johnabbott.cs616.noteapplication.ui.util.CircleView;
import ca.qc.johnabbott.cs616.noteapplication.ui.util.DatePickerDialogFragment;
import ca.qc.johnabbott.cs616.noteapplication.ui.util.DisplayUsersFragment;
import ca.qc.johnabbott.cs616.noteapplication.ui.util.TimePickerDialogFragment;

/**
 * A placeholder fragment containing a simple view.
 */
public class NoteActivityFragment extends Fragment implements View.OnClickListener, CompoundButton.OnCheckedChangeListener, OnCollaboratorAdded {

    //region GLOBALS (FIELDS)
    DisplayUsersFragment displayUsersFragment;

    private View root;
    private TextView reminder_textView;
    private Switch option_switch;
    private ImageView undo_imageView;
    private CircleView cirlce1;
    private CircleView cirlce2;
    private CircleView cirlce3;
    private CircleView cirlce4;
    private CircleView cirlce5;
    private CircleView cirlce6;
    private CircleView cirlce7;
    private CircleView circle8;
    private ConstraintLayout main_layout;
    private EditText title_editText;
    private EditText body_editText;

    private Note note;
    private Stack<Note> history;

    private Calendar calendar = Calendar.getInstance();

    //Sets undo state
    private Boolean revert = false;
    //endregion

    public NoteActivityFragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        //region Get Views
        root = inflater.inflate(R.layout.fragment_note, container, false);
        main_layout = root.findViewById(R.id.main_layout);
        title_editText = root.findViewById(R.id.title_editText);
        body_editText = root.findViewById(R.id.body_editText);
        reminder_textView = root.findViewById(R.id.reminder_textView);
        cirlce1 = root.findViewById(R.id.circle1_circleView);
        cirlce2 = root.findViewById(R.id.circle2_circleView);
        cirlce3 = root.findViewById(R.id.circle3_circleView);
        cirlce4 = root.findViewById(R.id.circle4_circleView);
        cirlce5 = root.findViewById(R.id.circle5_circleView);
        cirlce6 = root.findViewById(R.id.circle6_circleView);
        cirlce7 = root.findViewById(R.id.circle7_circleView);
        circle8 = root.findViewById(R.id.circle8_circleView);
        undo_imageView = root.findViewById(R.id.undo_imageView);
        option_switch = root.findViewById(R.id.showOptions_switch);
        //endregion

        //region Set Listeners
        title_editText.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence charSequence, int i, int i1, int i2) {

            }

            @Override
            public void onTextChanged(CharSequence charSequence, int i, int i1, int i2) {
                //If not undoing
                if(!revert){
                    //Update Note Title
                    note.setTitle(title_editText.getText().toString());
                    note.setModified(new Date());
                    update();
                }
            }

            @Override
            public void afterTextChanged(Editable editable) {

            }
        });
        body_editText.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence charSequence, int i, int i1, int i2) {

            }

            @Override
            public void onTextChanged(CharSequence charSequence, int i, int i1, int i2) {
                //If not undoing
                if(!revert){
                    //Update Note Body
                    note.setBody(body_editText.getText().toString());
                    note.setModified(new Date());
                    update();
                }
            }

            @Override
            public void afterTextChanged(Editable editable) {

            }
        });

        reminder_textView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                chooseDate();
            }
        });

        option_switch.setOnCheckedChangeListener(this);

        undo_imageView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                undo();
            }
        });

        cirlce1.setOnClickListener(this);
        cirlce2.setOnClickListener(this);
        cirlce3.setOnClickListener(this);
        cirlce4.setOnClickListener(this);
        cirlce5.setOnClickListener(this);
        cirlce6.setOnClickListener(this);
        cirlce7.setOnClickListener(this);
        circle8.setOnClickListener(this);
        //endregion

        //Get the fragment to populate
        displayUsersFragment = (DisplayUsersFragment) getChildFragmentManager().findFragmentById(R.id.users_Fragment);

        //Initialize Note
        note = new Note();
        note.setCategory(Category.NONE);
        setNote(note);
        note.setCreated(new Date());
        note.setHasReminder(false);
        note.setModified(new Date(-1));

        //Set Default Day to Tomorrow at 8 AM
        calendar.add(Calendar.DAY_OF_YEAR, 1);
        calendar.set(Calendar.HOUR_OF_DAY, 8);
        calendar.set(Calendar.MINUTE, 0);

        //Set the on request add click
        displayUsersFragment.setOnAddUserRequestedListener(new DisplayUsersFragment.OnAddUserRequestedListener() {
            @Override
            public void onAddUserRequested() throws DatabaseException {
                //Create the add collaborator dialog fragment off of the non collaborators
                new AddCollaboratorDialogFragment(getNonCollaborators(), NoteActivityFragment.this).show(getChildFragmentManager(), "addCollaborators");
            }
        });

        return root;
    }

    private void chooseDate(){
        //Create Date Picker
        DatePickerDialogFragment dialogFragment = DatePickerDialogFragment.create(calendar.getTime(), new DatePickerDialog.OnDateSetListener() {
            @Override
            public void onDateSet(DatePicker datePicker, int year, int month, int day) {
                //Get Date Data
                calendar.set(Calendar.YEAR, year);
                calendar.set(Calendar.MONTH, month);
                calendar.set(Calendar.DAY_OF_MONTH, day);
                //Call Time Picker
                chooseTime();
            }
        });
        //Call the Date Picker
        dialogFragment.show(getFragmentManager(), "datePicker");
    }

    private void chooseTime(){
        //Create Time Picker
        TimePickerDialogFragment dialogFragment = TimePickerDialogFragment.create(calendar.getTime(), new TimePickerDialog.OnTimeSetListener() {
            @Override
            public void onTimeSet(TimePicker timePicker, int hours, int minutes) {
                //Get Time Data
                calendar.set(Calendar.HOUR_OF_DAY, hours);
                calendar.set(Calendar.MINUTE, minutes);
                //Set the Date and Time to the note
                note.setReminder(calendar.getTime());
                note.setHasReminder(true);
                note.setModified(new Date());
                update();
                //Update UI
                SimpleDateFormat displayTime = new SimpleDateFormat("'Reminder:' EEEE, MMMM dd 'at' hh:mm aa ", Locale.US);
                reminder_textView.setText(displayTime.format(calendar.getTime()));
            }
        });
        //Call the Time Picker
        dialogFragment.show(getFragmentManager(), "datePicker");
    }

    @Override
    public void onCheckedChanged(CompoundButton compoundButton, boolean b) {
        //Get Option Layout
        LinearLayout options_layout = root.findViewById(R.id.options_layout);
        //If Checked
        if(b){
            //Make Layout Visible
            options_layout.setVisibility(View.VISIBLE);
        }
        else{
            //Make Layout Gone
            options_layout.setVisibility(View.GONE);
        }
    }

    @Override
    public void onClick(View view) {
        CircleView circle = (CircleView) view;

        //Gets the Background Color
        int color = Color.TRANSPARENT;
        Drawable background = main_layout.getBackground();
        if (background instanceof ColorDrawable)
            color = ((ColorDrawable) background).getColor();

        //If they have the same Background
        if(circle.getColor() == color){
            //Set the Background to Transparent
            main_layout.setBackgroundColor(Color.TRANSPARENT);
            //Set Category to Null
            note.setCategory(Category.NONE);
        }
        else{
            //Set the Background to the Circle Color
            main_layout.setBackgroundColor(circle.getColor());
            //Set Default Category
            Category cat = null;
            //Switch on the Circle to Get Category
            switch (circle.getId()){
                case R.id.circle1_circleView:
                    cat = Category.RED;
                    break;
                case R.id.circle2_circleView:
                    cat = Category.ORANGE;
                    break;
                case R.id.circle3_circleView:
                    cat = Category.YELLOW;
                    break;
                case R.id.circle4_circleView:
                    cat = Category.GREEN;
                    break;
                case R.id.circle5_circleView:
                    cat = Category.LIGHT_BLUE;
                    break;
                case R.id.circle6_circleView:
                    cat = Category.DARK_BLUE;
                    break;
                case R.id.circle7_circleView:
                    cat = Category.PURPLE;
                    break;
                case R.id.circle8_circleView:
                    cat = Category.BROWN;
                    break;
            }
            //Set Category
            note.setCategory(cat);
        }
        //Update Note
        note.setModified(new Date());
        update();
    }

    private void update(){
        //Create a new Note (Dereference Clone)
        Note tmp = note.clone();
        //Push New Note
        history.push(tmp);
    }

    private void undo() {
        //region Get History
        //If not only Base Case on Stack
        if(history.size() > 1){
            //Remove Current Note From Stack
            history.pop();
        }
        //Get Last Change
        note=history.peek().clone();
        //endregion

        updateUI(note);
    }

    private List<User> getCollaborators(){
        //Get the database handler
        final DatabaseHandler dbh = new DatabaseHandler(this.getContext());

        //Create the list to populate of users
        List<User> users = new ArrayList<>();

        try {
            //Get the list of all collaborators
            List<Collaborator> allCollaborators = dbh.getCollaboratorTable().readAll();

            //Loop through each collaborators
            for(Collaborator collab : allCollaborators){
                //If the collaborator is collaborating on the note then add it to the list
                if(collab.getNoteId() == note.getId())
                    users.add(dbh.getUserTable().read(collab.getUserId()));
            }
        } catch (DatabaseException e) {
        }

        //Return the built list
        return users;
    }

    private List<User> getNonCollaborators(){
        //Get the database handler
        final DatabaseHandler dbh = new DatabaseHandler(this.getContext());

        //Create the list of all users
        List<User> allUsers = new ArrayList<>();

        try{
            //Get all the collaborators fo the note
            List<User> collabs = getCollaborators();

            //Get the list of all of the users
            allUsers = dbh.getUserTable().readAll();

            //Loop through the collaborators of the note
            for(User collab : collabs){
                //Remove each of them from the list of users
                allUsers.remove(collab);
            }
        }
        catch (DatabaseException e){
        }

        //Return the list
        return allUsers;
    }

    public void share(){
        //Create Intent
        Intent share = new Intent();
        //Set Properties
        share.setAction(Intent.ACTION_SEND);
        share.putExtra(Intent.EXTRA_TEXT, note.toString());
        share.setType("text/plain");
        //Create the Shower
        Intent shareIntent = Intent.createChooser(share, null);
        //Call Intent
        startActivity(shareIntent);
    }

    public void updateUI(Note note){
        revert = true;
        //region Update Reminder
        if(note.isHasReminder()){
            //Update UI
            SimpleDateFormat displayTime = new SimpleDateFormat("'Reminder:' EEEE, MMMM dd 'at' hh:mm aa ", Locale.US);
            reminder_textView.setText(displayTime.format(note.getReminder()));
        }
        else{
            //Revert to base UI
            reminder_textView.setText("Add a reminder");
        }
        //endregion

        //region Update Text
        title_editText.setText(note.getTitle());
        body_editText.setText(note.getBody());
        //endregion

        //region Update Color
        //Get Base Color
        int color = Color.TRANSPARENT;
        //Avoids Null Crash :(
        if(note.getCategory() != Category.NONE){
            //Switch on Category to Get Color
            switch (note.getCategory()){
                case RED:
                    color=getResources().getColor(R.color.base08);
                    break;
                case ORANGE:
                    color=getResources().getColor(R.color.base09);
                    break;
                case YELLOW:
                    color=getResources().getColor(R.color.base0A);
                    break;
                case GREEN:
                    color=getResources().getColor(R.color.base0B);
                    break;
                case LIGHT_BLUE:
                    color=getResources().getColor(R.color.base0C);
                    break;
                case DARK_BLUE:
                    color=getResources().getColor(R.color.base0D);
                    break;
                case PURPLE:
                    color=getResources().getColor(R.color.base0E);
                    break;
                case BROWN:
                    color=getResources().getColor(R.color.base0F);
                    break;
            }
        }
        //Set Color
        main_layout.setBackgroundColor(color);
        //endregion

        revert = false;
    }

    public void setNote(Note note){
        //Rests history
        history = new Stack<>();

        //Set note
        this.note = note;

        //Update the UI
        updateUI(note);

        //Update the database
        update();

        //Set the users
        displayUsersFragment.setUsers(getCollaborators());
    }

    public Note getNote(){
        return note;
    }

    @Override
    public void onCollaboratorAdded(User user) throws DatabaseException {
        //Get the database handler
        DatabaseHandler dbh = new DatabaseHandler(this.getContext());

        //Create the new collaborator entity off of the ids
        Collaborator newCollab = new Collaborator();
        newCollab.setId((long) (dbh.getCollaboratorTable().readAll().size() + 1));
        newCollab.setNoteId(note.getId());
        newCollab.setUserId(user.getId());

        //Create the entity in the table
        dbh.getCollaboratorTable().create(newCollab);

        //Set the note users
        displayUsersFragment.setUsers(getCollaborators());
    }
}