# COMP_346_ASG1

Brandon Cameron - 40175294
Sean Meath - 4017529X

There's a blocker, blocker lock, parkBlocker in all thread objs built in, which we could use if we need.


Server: Contains all data
  numberOfTransactions
  numberOfAccounts
  transaction data
  accounts array
  
  Contains objNetwork:
    Contains:
      Name
      Priority
      
Sender: 
  clientOp
  Name
  priority
  
Network:
  buffers
  client & server connection status
  incoming packets array
  outgoing packets array
  input buffer & output buffer
  network status
  
  
getoutputIndexServer( ) == getinputIndexClient( ) :-> setInBufferStatus("empty"); (Causing the infinite loop, why is it hitting this first?)


  
