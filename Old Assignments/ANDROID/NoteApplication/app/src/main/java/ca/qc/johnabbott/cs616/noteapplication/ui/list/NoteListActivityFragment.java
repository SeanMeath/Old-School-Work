package ca.qc.johnabbott.cs616.noteapplication.ui.list;
import androidx.annotation.RequiresApi;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout;

import android.annotation.SuppressLint;
import android.app.AlertDialog;
import android.app.DatePickerDialog;
import android.app.TimePickerDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Rect;
import android.os.Build;
import android.os.Bundle;
import android.view.ActionMode;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.DatePicker;
import android.widget.Spinner;
import android.widget.TimePicker;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Calendar;
import java.util.Comparator;
import java.util.Date;
import java.util.List;

import ca.qc.johnabbott.cs616.noteapplication.model.Collaborator;
import ca.qc.johnabbott.cs616.noteapplication.model.NoteAdapter;
import ca.qc.johnabbott.cs616.noteapplication.R;
import ca.qc.johnabbott.cs616.noteapplication.model.Note;
import ca.qc.johnabbott.cs616.noteapplication.model.DatabaseHandler;
import ca.qc.johnabbott.cs616.noteapplication.model.OnNoteClickListener;
import ca.qc.johnabbott.cs616.noteapplication.model.User;
import ca.qc.johnabbott.cs616.noteapplication.model.UserData;
import ca.qc.johnabbott.cs616.noteapplication.networking.HttpRequest;
import ca.qc.johnabbott.cs616.noteapplication.networking.HttpRequestTask;
import ca.qc.johnabbott.cs616.noteapplication.networking.HttpResponse;
import ca.qc.johnabbott.cs616.noteapplication.networking.OnErrorListener;
import ca.qc.johnabbott.cs616.noteapplication.networking.OnResponseListener;
import ca.qc.johnabbott.cs616.noteapplication.sqlite.DatabaseException;
import ca.qc.johnabbott.cs616.noteapplication.ui.NotesApplication;
import ca.qc.johnabbott.cs616.noteapplication.ui.editor.NoteActivity;
import ca.qc.johnabbott.cs616.noteapplication.ui.util.DatePickerDialogFragment;
import ca.qc.johnabbott.cs616.noteapplication.ui.util.TimePickerDialogFragment;

/**
 * Fragment to hold the notes view, shows all notes in the database:
 * Allows for sorting based on a few criteria,
 * Allows the deletion of notes,
 * Allows for the edit of the reminder of notes
 */
public class NoteListActivityFragment extends Fragment implements OnNoteClickListener {

    public NoteListActivityFragment() {
    }

    //region GLOBALS
    View root;
    Spinner noteSpinner;
    RecyclerView noteRecyclerView;
    NoteAdapter adapter;
    Calendar calendar = Calendar.getInstance();
    Note sentNote = new Note();
    List<Note> userNotes;

    AlertDialog.Builder alertDialogBuilder;
    NotesApplication application;
    Note noteToAdd;
    SwipeRefreshLayout swipeRefreshLayout;
    //endregion

    @SuppressLint("ClickableViewAccessibility")
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        //Get the layout for the note list and sets up it's UI
        root = inflater.inflate(R.layout.fragment_note_list, container, false);

        //Get the views that will be used
        noteSpinner = root.findViewById(R.id.category_Spinner);
        noteRecyclerView = root.findViewById(R.id.note_RecyclerView);

        //Populate the spinners sorting list
        //region SPINNER SORTING LIST
        List<String> sortingOptions = new ArrayList<>();
        sortingOptions.add("Creation Date");
        sortingOptions.add("Category");
        sortingOptions.add("Reminder");
        sortingOptions.add("Title");
        sortingOptions.add("Last Modified");
        //endregion

        //Populate and initialise the spinner with it's sorting list
        ArrayAdapter<String> optionAdapter = new ArrayAdapter<>(getContext(), R.layout.list_item_sort, R.id.option_TextView);
        optionAdapter.addAll(sortingOptions);
        noteSpinner.setAdapter(optionAdapter);

        //Set the sorting function to run when a sorting option is chosen
        noteSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @RequiresApi(api = Build.VERSION_CODES.N)
            @Override
            public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                //Sort on Creation Date
                if(l == 0){
                    userNotes.sort(new Comparator<Note>() {
                        @Override
                        public int compare(Note n1, Note n2) {
                            return n1.getCreated().compareTo(n2.getCreated());
                        }
                    });
                }
                //Sort on Category
                else if(l == 1){
                    userNotes.sort(new Comparator<Note>() {
                        @Override
                        public int compare(Note n1, Note n2) {
                            return n1.getCategory().compareTo(n2.getCategory());
                        }
                    });
                }
                //Sort on Reminder
                else if(l == 2){
                    userNotes.sort(new Comparator<Note>() {
                        @Override
                        public int compare(Note n1, Note n2) {
                            //If the reminder does not exist / the other one has priority
                            if(!n1.isHasReminder())
                                return 1;
                            if(!n2.isHasReminder())
                                return -1;
                            return n1.getReminder().compareTo(n2.getReminder());
                        }
                    });
                }
                //Sort on Title
                else if(l == 3){
                    userNotes.sort(new Comparator<Note>() {
                        @Override
                        public int compare(Note n1, Note n2) {
                            return n1.getTitle().compareTo(n2.getTitle());
                        }
                    });
                }
                //Sort on Modified
                else if(l == 4){
                    userNotes.sort(new Comparator<Note>() {
                        @Override
                        public int compare(Note n1, Note n2) {
                            return n1.getModified().compareTo(n2.getModified());
                        }
                    });
                }
                //Update the recycler view (through it's adapter)
                adapter.notifyDataSetChanged();
                noteRecyclerView.setLayoutManager(new GridLayoutManager(getContext(), 2));
            }

            @Override
            public void onNothingSelected(AdapterView<?> adapterView) {

            }
        });

        //Get the application reference class
        application = (NotesApplication) getActivity().getApplication();

        //Get the refresh layout
        swipeRefreshLayout = root.findViewById(R.id.swiperefresh);

        //Set on refresh to update the UI
        swipeRefreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
            @Override
            public void onRefresh() {
                //Update the UI
                getNotes();
            }
        });

//        alertDialogBuilder = new AlertDialog.Builder(root.getContext());
//        alertDialogBuilder.setPositiveButton("Try Again!", new DialogInterface.OnClickListener(){
//            @Override
//            public void onClick(DialogInterface dialog, int which) {
//
//            }
//        });
//        alertDialogBuilder.setNegativeButton("Stop Attempts", new DialogInterface.OnClickListener(){
//
//            @Override
//            public void onClick(DialogInterface dialog, int which) {
//
//            }
//        });

        //Get the list of notes
        getNotes();

        //Set the recycler view with the list of notes
        adapter = new NoteAdapter(new ArrayList<>(), this);
        noteRecyclerView.setAdapter(adapter);
        noteRecyclerView.setLayoutManager(new GridLayoutManager(getContext(), 2));

        return root;
    }

    //Once a note option was clicked show the menu of options
    @RequiresApi(api = Build.VERSION_CODES.M)
    public void onClickNote(final long id, final int position, final float x, final float y){
        getActivity().startActionMode(new ActionMode.Callback2() {
            @Override
            public boolean onCreateActionMode(ActionMode actionMode, Menu menu) {
                //Create the UI for the menu
                actionMode.getMenuInflater().inflate(R.menu.menu_note_float, menu);
                return true;
            }

            @Override
            public boolean onPrepareActionMode(ActionMode actionMode, Menu menu) {
                return false;
            }

            //On a menu option being clicked
            @RequiresApi(api = Build.VERSION_CODES.Q)
            @Override
            public boolean onActionItemClicked(ActionMode actionMode, MenuItem menuItem) {
                popUpMenu(position, actionMode, menuItem);
                return true;
            }

            @Override
            public void onDestroyActionMode(ActionMode actionMode) {

            }

            @Override
            public void onGetContentRect(ActionMode mode, View view, Rect outRect) {
                outRect.set((int)x, (int)y, (int)x, (int)y);
            }
        }, ActionMode.TYPE_FLOATING);
    }

    private void popUpMenu(final int position, ActionMode actionMode, MenuItem menuItem){
        Note note = userNotes.get(position);

        switch (menuItem.getItemId()){
            //If the edit is chosen
            case R.id.edit_MenuItem:

                Intent intent = new Intent(getContext(), NoteActivity.class);
                intent.putExtra("initial_note", note);
                intent.putExtra("position", position);

                //Set the sent note (For undo)
                sentNote = note;

                //Start the activity
                startActivityForResult(intent, 1);
                actionMode.finish();
                break;
            //If the reminder is chosen
            case R.id.reminder_MenuItem:

                //Attempt to get the note to edit and call the datepicker to change the time
                chooseDate(note, position);

                actionMode.finish();
                break;
            //If the trash is chosen
            case R.id.trash_MenuItem:
                //Remove the note by position from the data as well as the web-server
                //Then update the recycler view and database to cause the change to take place
                delete(position);

                actionMode.finish();
                break;
            //If close was chosen, close :)
            case R.id.close_MenuItem:
                actionMode.finish();
                break;
        }
    }

    private void chooseDate(final Note note, final int position){
        //Create Date Picker
        DatePickerDialogFragment dialogFragment = DatePickerDialogFragment.create(calendar.getTime(), new DatePickerDialog.OnDateSetListener() {
            @Override
            public void onDateSet(DatePicker datePicker, int year, int month, int day) {
                //Get Date Data
                calendar.set(Calendar.YEAR, year);
                calendar.set(Calendar.MONTH, month);
                calendar.set(Calendar.DAY_OF_MONTH, day);
                //Call Time Picker
                chooseTime(note, position);
            }
        });
        //Call the Date Picker
        dialogFragment.show(getFragmentManager(), "datePicker");
    }

    private void chooseTime(final Note note, final int position){
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
                //Attempt to update the note in the database and the data
                update(note, position);
            }
        });
        //Call the Time Picker
        dialogFragment.show(getFragmentManager(), "datePicker");
    }

    public void getNotes(){
        //Get the URL for receiving the notes for the user
        String repoUrl = application.getUserNotesUrl();

        //Build the request
        HttpRequest request = new HttpRequest(repoUrl, HttpRequest.Method.GET);
        HttpRequestTask httpRequestTask = new HttpRequestTask();

        //Set error and success actions
        httpRequestTask.setOnErrorListener(new OnErrorListener() {
            //If errors occur display error to the user and reset the list
            @Override
            public void onError(Exception error) {
                Toast.makeText(getContext(), "An error has occured when processing the request, please try again.", Toast.LENGTH_LONG).show();
                userNotes = new ArrayList();
                updateNotes();
            }
        });
        httpRequestTask.setOnResponseListener(new OnResponseListener<HttpResponse>() {
            //if all goes well, set the notes list and update UI
            @Override
            public void onResponse(HttpResponse response) {
                String jsonNotes = response.getResponseBody();
                userNotes = Arrays.asList(Note.parseArray(jsonNotes));
                updateNotes();
            }
        });

        //Send the request
        httpRequestTask.execute(request);

        //Stop the process of refreshing
        swipeRefreshLayout.setRefreshing(false);
    }

    public void update(Note note, int position){
        //Get the URL for updating notes in the server
        String repoUrl = application.getUpdateUrl(note.getUuid());

        //Build the request
        HttpRequest request = new HttpRequest(repoUrl, HttpRequest.Method.PUT);

        //Place the note in json format in the body
        request.setRequestBody("application/json", note.format());
        HttpRequestTask httpRequestTask = new HttpRequestTask();

        //Set error and success actions
        httpRequestTask.setOnErrorListener(new OnErrorListener() {
            //If error occurs, send a message warning user
            @Override
            public void onError(Exception error) {
                Toast.makeText(getContext(), "An error has occured when processing the request, please try again.", Toast.LENGTH_SHORT).show();
            }
        });
        httpRequestTask.setOnResponseListener(new OnResponseListener<HttpResponse>() {
            //If all goes well, update the list and update the UI
            @Override
            public void onResponse(HttpResponse response) {
                userNotes.set(position, note);
                updateNotes();
            }
        });

        //Send the request
        httpRequestTask.execute(request);
    }

    public void add(Note note){
        //Get the URL for adding notes in the server
        String repoUrl = application.getAddNoteUrl();

        //Build the request
        HttpRequest request = new HttpRequest(repoUrl, HttpRequest.Method.POST);

        //Place the note in json format in the body
        request.setRequestBody("application/json", note.format());
        HttpRequestTask httpRequestTask = new HttpRequestTask();

        //Set note to add in future if all goes well
        noteToAdd = note;

        //Set error and success actions
        httpRequestTask.setOnErrorListener(new OnErrorListener() {
            //If error occurs, send a message warning user
            @Override
            public void onError(Exception error) {
                Toast.makeText(getContext(), "An error has occured when processing the request, please try again.", Toast.LENGTH_SHORT).show();
            }
        });
        httpRequestTask.setOnResponseListener(new OnResponseListener<HttpResponse>() {
            //If all goes well, add the current user to its collaborators
            @Override
            public void onResponse(HttpResponse response) {
                //Get the location of the created note (Includes its uuid)
                String uuid = response.getHeaders().get("Location").get(0);
                addCollaborator(uuid);
            }
        });

        //Send the request
        httpRequestTask.execute(request);
    }

    public void addCollaborator(String noteLink){
        //Get the URL for adding collaborators in the server
        String repoUrl = application.getAddCollaboratorUrl();

        //Get the reference url for the user
        String userLink = application.getUserUrl();

        //Create a collaborator with the user and note links
        Collaborator collab = new Collaborator().setNote(noteLink).setUser(userLink);

        //Build the request
        HttpRequest request = new HttpRequest(repoUrl, HttpRequest.Method.POST);

        //Place the collaborator in json format in the body
        request.setRequestBody("application/json", collab.format());
        HttpRequestTask httpRequestTask = new HttpRequestTask();

        //Set error and success actions
        httpRequestTask.setOnErrorListener(new OnErrorListener() {
            //If error occurs, send a message warning user
            @Override
            public void onError(Exception error) {
                Toast.makeText(getContext(), "An error has occured when processing the request, please try again.", Toast.LENGTH_SHORT).show();
            }
        });
        httpRequestTask.setOnResponseListener(new OnResponseListener<HttpResponse>() {
            @Override
            public void onResponse(HttpResponse response) {
                //CRASHES WHEN I ADD :(
                //userNotes.add(noteToAdd);
                //updateNotes();
            }
        });
        httpRequestTask.execute(request);
    }

    public void delete(int position){
//        userNotes.remove(position);
//        updateNotes();
    }

    public void undo(String type, int position){
        if(type == "Updated"){
            //Return to the initial note sent
            update(sentNote, position);
        }
        else{
            //Delete the position created
            delete(userNotes.size()-1);
        }
    }

    public void updateNotes(){
        adapter = new NoteAdapter(userNotes, this);
        noteRecyclerView.setAdapter(adapter);
        noteRecyclerView.setLayoutManager(new GridLayoutManager(getContext(), 2));
    }
}