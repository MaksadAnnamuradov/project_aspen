import React from 'react';
import { Charity } from '../../models/CharityModel';
import { Button } from "@material-ui/core";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import { actionCreators } from "../../store/GlobalAdmin/actions";
import { ApplicationState } from "../../store";

interface AddUpdateCharityFormProps {
    Charity?: Charity,
    adminAddCharity: typeof actionCreators.adminAddCharity,
}

const AddUpdateCharityForm:React.FC<AddUpdateCharityFormProps> = props => {
    let [charityName, setCharityName] = React.useState("");
    let [charityDomain, setCharityDomain] = React.useState("");
    let [charityDescription, setCharityDescription] = React.useState("");
    return (<>
        <form>
            <h3>Charity Name</h3>
            <input type="text" onChange={event => setCharityName(event.target.value)}></input>
            <h3>Charity Domain</h3>
            <input type="text" onChange={event => setCharityDomain(event.target.value)}></input>
            <h3>Charity Description</h3>
            <input type="text" onChange={event => setCharityDescription(event.target.value)}></input>
        </form>
        <Button onClick={()=>props.adminAddCharity(new Charity("",charityName,charityDomain,charityDescription))}>Submit</Button>
       </>
    )
}

AddUpdateCharityForm.defaultProps = {
    Charity: new Charity("0","","",""),
}

const mapStateToProps = (state: ApplicationState) => {
    return {
    }
}

export default connect(
  mapStateToProps,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(AddUpdateCharityForm);