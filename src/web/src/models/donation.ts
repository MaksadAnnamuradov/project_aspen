export default class Donation {
  id?: number;
  eventID: number;
  teamID: number;
  personID: number;
  date: string;
  amount: number;
  isPending: boolean;

  constructor(
    eventID: number,
    teamID: number,
    personID: number,
    date: string,
    amount: number,
  ) {
    this.eventID = eventID;
    this.teamID = teamID;
    this.personID = personID;
    this.date = date;
    this.amount = amount;
    this.isPending = false;
    this.id = undefined;
  }
}
