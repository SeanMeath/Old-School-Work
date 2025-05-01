package ca.qc.johnabbott.cs406.search;
import ca.qc.johnabbott.cs406.terrain.Direction;
import ca.qc.johnabbott.cs406.terrain.Location;
import ca.qc.johnabbott.cs406.terrain.Terrain;
import ca.qc.johnabbott.cs406.collections.Queue;

import java.awt.*;

public class BFS implements Search {

    private Memory memory;
    private Location solution;
    private boolean foundSolution;
    private Terrain terrain;
    private Queue <Location> toCheck;

    public BFS() {
    }

    @Override
    public void solve(Terrain terrain) {

        // set the default to false to maintain consistency
        foundSolution = false;

        // get the terrain to be used
        this.terrain = terrain;

        // set up the queue
        this.toCheck = new Queue(terrain.getHeight() * terrain.getWidth());

        // track locations we've been to using our terrain "memory"
        memory = new Memory();

        // track the current search location, starting at the terrain start location.
        toCheck.enqueue(terrain.getStart());
        Location current;
        memory.setFromDirection(terrain.getStart(), null);

        // tracks the location to be checked.
        Location check;

        // while there are more locations to check
        while(!toCheck.isEmpty()){

            // get a new location to check
            current = toCheck.dequeue();

            // check if its the goal
            if(current.equals(terrain.getGoal())){
                foundSolution = true;
                break;
            }

            // set the current tile to visited
            memory.setColor(current, Color.BLACK);

            // goes through all the directions
            for(Direction dir: Direction.getClockwise()){

                // gets the direction to check
                check = current.get(dir);

                // check if direction is suitable
                if(!terrain.isBlocked(check) && memory.getColor(check) == Color.WHITE) {

                    //set the memory for trail
                    memory.setFromDirection(check, dir.opposite());
                    memory.setColor(check, Color.GREY);

                    // add the new area to the queue to check
                    toCheck.enqueue(check);
                }
            }
            System.out.println(memory.toString());
            System.out.println(current);
        }

    }

    // flips the order of the directions to allow pathing
    public void flip(){

        // holds the direction to add from the next one
        Direction nextDir = null;

        // gets the end
        solution = terrain.getGoal();

        //while there are more to the pathing
        while(memory.getFromDirection(solution) != null){

            // get the next direction to add to the previous position
            nextDir = memory.getFromDirection(solution).opposite();

            // move backwards
            solution = solution.get(memory.getFromDirection(solution));

            // set the to direction of the new position
            memory.setToDirection(solution, nextDir);
        }

        // set the first movement for the start position
        memory.setToDirection(solution, nextDir);
    }

    @Override
    public boolean hasSolution() {
        return foundSolution;
    }

    @Override
    public void reset() {
        // start the traversal of our path to the terrain's end.
        flip();
        solution = terrain.getStart();
    }

    @Override
    public boolean hasNext() {
        // we're only done when we get to the terrain end
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

