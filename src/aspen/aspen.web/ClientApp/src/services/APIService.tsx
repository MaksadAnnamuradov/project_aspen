import { IAPIService } from "./IAPIService";
import { CharityHomePage } from "../models/CharityHomePageModel";
import { Charity } from "../models/CharityModel";
import { IDomainService } from "./IDomainService";
import { Theme } from "../models/Theme";

const url = "http://192.168.107.128:5000" 

// const url = process.env.REACT_APP_API_URL 
const globaladmindomain = process.env.REACT_APP_GLOBAL_ADMIN_DOMAIN

export class APIService implements IAPIService {
    IDomainService: IDomainService;

    constructor(IDomainService: IDomainService) {
        this.IDomainService = IDomainService
    }

    public async GetCharityThemeByDomain():Promise<Theme>{
        
    }


    public async GetCharityHomePage(): Promise<CharityHomePage> {
        let domain = this.IDomainService.GetDomain();
        let headers = { "Content-Type": "application/json" };
        let newurl = url + "/charity/getbydomain?domain="+ domain;
        let response = await fetch(newurl, {
            method: "GET",
            headers: headers
        })

        console.error("called charity")
        console.error(response);
        let responseJson = await response.json()

        console.warn(responseJson);

        if(responseJson.Status == "Success"){
            let id = responseJson["data"]["id"];
            let name = responseJson["data"]["name"];
            let domain = responseJson["data"]["domain"];
            let description = responseJson["data"]["description"];
            let charityObject = new Charity(id, name, domain, description)
            
            // TODO get the theme and place it here. 
            let fontFamily = responseJson["data"]["fontFamily"];
            let PrimaryMainColor = responseJson["data"]["PrimaryMainColor"];
            let PrimaryLightColor = responseJson["data"]["PrimaryLightColor"];
            let PrimaryContrastTextColor = responseJson["data"]["PrimaryContrastTextColor"];
            let SecondaryMainColor = responseJson["data"]["SecondaryMainColor"];
            let theme =  new Theme(PrimaryMainColor, PrimaryLightColor, PrimaryContrastTextColor, SecondaryMainColor, fontFamily)
            return new CharityHomePage(theme, charityObject)
        }
        //TODO: make a second api call to get the theme and remove the theme from the first api call 
        let theme = new Theme("#438f00","#67cc0e","#FFFFFF", "#608045","Arial");
        let charityObject = new Charity(1,"FAILED","FAILED","FAILED")
        let charityHomePage = new CharityHomePage(theme,charityObject);
        return charityHomePage;
    }


    public async GetAllCharities(): Promise<Charity[]> {
        let headers = { "Content-Type": "application/json" };
        let newurl = url + "admin/charity/GetAll"
        let response = await fetch(newurl, {
            method: "GET",
            headers: headers
        })

        let responseJson = await response.json()
        console.error(responseJson)

        return [new Charity(1,"Kylers penguin's","kyler.com","this is where the awesome penguin's live")]
    }

    public async GetCharityByID(ID: string): Promise<Charity> {
        let headers = { "Content-Type": "application/json" };
        let newurl = url + "/Charity/Get?Id="+ID
        let response = await fetch(newurl, {
            method: "GET",
            headers: headers
        })

        let responseJson = await response.json();

        let c = new Charity("","Kylers penguin's","kyler.com","this is where the awesome penguin's live");
        return c 
    }

    //this is now working but not using the domain service
    public async GetCharityByDomain(): Promise<Charity> {
        try{
            console.log("In GetCharityByDomain");
            let domain = "kylerspenguins3.com";
            let headers = { "Content-Type": "application/json" };
            let newurl = url + "/Charity/Get?domain="+domain
            let response = await fetch(newurl, {
                method: "GET",
                headers: headers
            })

            let responseJson = await response.json();
            if(responseJson.status == "Success"){
                let id = responseJson.data.charityId;
                let name = responseJson.data.charityName;
                let description = responseJson.data.charityDescription;
                let res_domains = responseJson.data.domains;
                let charityObject = new Charity(id, name, res_domains, description);
                return charityObject
            }else{
                throw Error("Domain not found");
            }
        }catch(e){
            console.error("error:"+e);
            let c = new Charity("","error","error","error");
            return c;
        }
    }

    //This works successfully -kyler
    public async PostCreateCharity(charity : Charity): Promise<boolean> {
        try{
            let headers = { "Content-Type": "application/json" };
            let body = JSON.stringify(charity);
            console.error("body:"+body);
            let newurl = url + "/Admin/Charity/Create"
            let response = await fetch(newurl, {
                method: "POST",
                mode:"cors",
                headers: headers,
                body: body
            });
            let responseJson = await response.json();
            if(responseJson.status == "Success"){
                console.error("We added the charity successfully");
                return true;
            }else{
                console.error("adding the charity failed");
                return false;
            }
        }catch(e){
            console.error("adding the charity failed");
            return false;
        }  
    }
    //this is talking to the api correctly
    public async PostUpdateCharity(charity: Charity): Promise<boolean> {
        try{
            let headers = { "Content-Type": "application/json" };
            let body = JSON.stringify(charity);
            let newurl = url + "/Admin/Charity/Update"
            let response = await fetch(newurl, {
                method: "POST",
                mode:"cors",
                headers: headers,
                body: body
            })
            let responseJson = await response.json();
            if(responseJson.status == "Success"){
                console.error("We Updated the charity successfully");
                return true;
            }else{
                console.error("Updating the charity failed");
                return false;
            }
        }catch(e){
            console.error("Updating the charity failed");
            return false;
        }   

    }

    public async PostDeleteCharity(charity: Charity): Promise<boolean> {
        let headers = { "Content-Type": "application/json" };
        let body = JSON.stringify(charity);
        let newurl = url + "/Charity/Delete"
        let response = await fetch(newurl, {
            method: "POST",
            headers: headers,
            body: body
        })

        let responseJson = await response.json();
        return true;
    }

}