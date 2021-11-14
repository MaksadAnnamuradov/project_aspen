import axios from "axios";
import Team from "../models/team"
const url = `${process.env.PUBLIC_URL}/api/teams`;

const getAllTeams = async () =>{
    const res = await axios.get<Team[]>(url);
    return res.data;
}

const createTeam = async (team :Team) =>{
    const res = await axios.post<Team>(url + `?eventId=${team.eventID}`, team)
    return res.data;
}


const teamService={
    getAllTeams,
    createTeam
}
export default teamService; 
