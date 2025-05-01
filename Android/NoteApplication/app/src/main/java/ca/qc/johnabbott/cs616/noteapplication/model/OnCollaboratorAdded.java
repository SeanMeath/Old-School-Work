package ca.qc.johnabbott.cs616.noteapplication.model;

import ca.qc.johnabbott.cs616.noteapplication.sqlite.DatabaseException;

public interface OnCollaboratorAdded {
    public void onCollaboratorAdded(User user) throws DatabaseException;
}