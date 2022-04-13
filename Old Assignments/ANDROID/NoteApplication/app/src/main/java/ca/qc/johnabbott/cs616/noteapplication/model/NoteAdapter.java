package ca.qc.johnabbott.cs616.noteapplication.model;
import android.annotation.SuppressLint;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import java.util.List;

import ca.qc.johnabbott.cs616.noteapplication.R;

public class NoteAdapter extends RecyclerView.Adapter<NoteViewHolder> {

    //GLOBALS
    private List<Note> data;
    private OnNoteClickListener mainActivity;

    @SuppressLint("ClickableViewAccessibility")
    public NoteAdapter(List<Note> data, OnNoteClickListener mainActivity) {
        //Gets the data for the adapter
        this.data = data;

        //Saves the main activity fragment to pass onto the holder (to use the function in the fragment)
        this.mainActivity = mainActivity;
    }

    @NonNull
    @Override
    public NoteViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        //Get and initialise the UI of the list items
        View root = LayoutInflater.from(parent.getContext()).inflate(R.layout.list_item_note, parent, false);

        //Create the view holders for the (notes/tiles)
        NoteViewHolder holder = new NoteViewHolder(root, mainActivity);

        return holder;
    }

    @Override
    public void onBindViewHolder(@NonNull NoteViewHolder holder, int position) {
        //Get the note from the data to be set into the holder
        Note note = data.get(position);

        //Set the note into the holder
        holder.set(note, position);
    }

    @Override
    public int getItemCount() {
        //Returns the size of the dataset
        return data.size();
    }
}
