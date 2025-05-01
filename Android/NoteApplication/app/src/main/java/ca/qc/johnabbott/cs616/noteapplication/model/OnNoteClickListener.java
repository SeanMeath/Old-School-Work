package ca.qc.johnabbott.cs616.noteapplication.model;
//Interface to allow the usage of the onClickNote function from the NoteViewHolder
public interface OnNoteClickListener {
    public void onClickNote(long id, int position, float x, float y);
}
