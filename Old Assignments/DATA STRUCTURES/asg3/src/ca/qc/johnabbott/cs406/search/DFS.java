package ca.qc.johnabbott.cs406.search;

import ca.qc.johnabbott.cs406.terrain.Direction;
import ca.qc.johnabbott.cs406.terrain.Location;
import ca.qc.johnabbott.cs406.terrain.Terrain;

public class DFS implements Search {

    private Memory memory;
    private Location solution;
    private boolean foundSolution;
    private Terrain terrain;

    public DFS() {
    }

    @Override
    public void solve(Terrain terrain) {

        // get the terrain to be used
        this.terrain = terrain;

        // track locations we've been to using our terrain "memory"
        memory = new Memory();

        // track the current search location, starting at the terrain start location.
        Location current = terrain.getStart();

        // record that we've been here
        memory.setColor(current, Color.BLACK);

        // tracks the location to be checked.
        Location check;

        // direction found
        Direction move = null;

        // loops until you find the goal
        while(!current.equals(terrain.getGoal())) {

            // goes through all the directions until a suitable one is found
            for(Direction dir: Direction.getClockwise()){
                move = dir;

                // gets the direction to check
                check = current.get(dir);

                // check if direction is blocked
                if(terrain.isBlocked(check) || memory.getColor(check) != Color.WHITE) {
                    move = null;
                }
                else{
                    break;
                }
            }

            // if no suitable directions
            if(move == null){

                // if at start then it's impossible
                if(current.equals(terrain.getStart())){
                    foundSolution = false;
                    return;
                }

                // go to last place visited and re-loop
                current = current.get(memory.getFromDirection(current));
                System.out.println(memory.toString());
                System.out.println(current);
                continue;
            }
            // record the step we've taken to memory to recreate the solution in the later traversal.
            memory.setToDirection(current, move);
            // step
            current = current.get(move);
            // record the last step done to get here
            memory.setFromDirection(current, move.opposite());
            // record that we've been here
            memory.setColor(current, Color.BLACK);
            System.out.println(memory.toString());
            System.out.println(current);
        }
        // found a solution
        foundSolution = true;
    }

    @Override
    public boolean hasSolution() {
        return foundSolution;
    }

    @Override
    public void reset() {
        // start the traversal of our path to the terrain's end.
        solution = terrain.getStart();
    }

    @Override
    public boolean hasNext() {
        // we're only done when we get to the terrain end.
        return !solution.equals(terrain.getGoal());
    }

    @Override
    public Direction next() {
        // recall the direction at this location, move to the corresponding location and return it.
        Direction direction = memory.getToDirection(solution);
        solution = solution.get(direction);
        return direction;
    }
}
