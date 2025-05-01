package ca.qc.johnabbott.cs616.noteapplication.ui.list;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout;

import com.google.android.material.floatingactionbutton.FloatingActionButton;
import com.google.android.material.snackbar.Snackbar;
import ca.qc.johnabbott.cs616.noteapplication.R;
import ca.qc.johnabbott.cs616.noteapplication.model.Note;
import ca.qc.johnabbott.cs616.noteapplication.sqlite.DatabaseException;
import ca.qc.johnabbott.cs616.noteapplication.ui.editor.NoteActivity;

public class NoteListActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_note_list);
        Toolbar toolbar = findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        FloatingActionButton addNote = findViewById(R.id.addNote_FloatingActionButton);
        addNote.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(view.getContext(), NoteActivity.class);
                startActivityForResult(intent, 2);
            }
        });
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        //Get the Intent data
        final Note note = data.getParcelableExtra("final_note");
        final int position = data.getIntExtra("position", 0);

        //Get the fragment
        final NoteListActivityFragment fragment = (NoteListActivityFragment) getSupportFragmentManager().findFragmentById(R.id.fragment);

        String type = "";

        //If the note was updated
        if(position >= 0) {
            fragment.update(note, position);
            type = "Updated";
        }
        //If the note was added
        else{
            fragment.add(note);
            type = "Added";
        }

        //Create the Snackbar
        FloatingActionButton addNote = findViewById(R.id.addNote_FloatingActionButton);
        final String finalType = type;
        Snackbar.make(addNote, "Note " + type, Snackbar.LENGTH_LONG).setAction("Undo", new View.OnClickListener(){
            @Override
            public void onClick(View view){
                //Upon click, undo
                fragment.undo(finalType, position);
            }
        }).show();
    }
}