package ca.qc.johnabbott.cs616.noteapplication.ui.editor;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import ca.qc.johnabbott.cs616.noteapplication.R;
import ca.qc.johnabbott.cs616.noteapplication.model.Note;

public class NoteActivity extends AppCompatActivity {

    private NoteActivityFragment fragment;
    private int position;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_note);

        //Get Toolbar
        Toolbar toolbar = findViewById(R.id.toolbar_toolbar);
        setSupportActionBar(toolbar);

        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        //Make the Toolbar Exist (Inflate)
        fragment = (NoteActivityFragment) getSupportFragmentManager().findFragmentById(R.id.fragment);

        //Get the Intent that launched
        Intent intent = getIntent();

        //If the initial note is set
        if(intent.hasExtra("initial_note")){
            //Set the note to the given one
            fragment.setNote((Note) intent.getParcelableExtra("initial_note"));
        }

        //get the position (or -1 if created)
        position = intent.getIntExtra("position", -1);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        //Make the Options Exist (Inflate)
        getMenuInflater().inflate(R.menu.menu_note, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            //Call the share function
            case R.id.share_MenuItem:
                fragment.share();
                break;
            //Returns to the List Activity Via Intent With the new note
            default:
                Intent intent = getIntent();
                //Sends the note and it's position into the intent
                intent.putExtra("final_note", fragment.getNote());
                intent.putExtra("position", position);

                //Return that all worked
                setResult(Activity.RESULT_OK, intent);
                finish();
                return true;
        }
        return super.onOptionsItemSelected(item);
    }
}
