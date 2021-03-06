import React, { Component } from "react"
import { connect } from "react-redux"
import { bindActionCreators } from "redux"
import { withStyles } from "@material-ui/core/styles"
import { FormControl, InputLabel, Select, Input, Chip, MenuItem, TextField } from "@material-ui/core"
import { elementList, APP_TEXTS } from "../../../constants"  
import { addSpellAction } from "../../../actions"
import { AddRules, Button } from ".."

const styles = {
    menuItem: {
    },
    formWrapper: {
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        width: "80%",
    },
    numericInputWrapper: {
        display: "flex",
        justifyContent: "space-between",
        width: "100%"
    },
    numericInput: {
        width: "45%",
    },
    addRulesComponentWrapper: {
        display: "flex",
        flexDirection: "column",
        alignSelf: "center",
    }
}

class AddSpellComponent extends Component {

    constructor(props) {
        super(props)

        this.state = {
            spellname:"",
            baseDamage: 0,
            baseManaCost: 0,
            elements: 0,
            selectedElements: []
        }

        this.handleChange = this.handleChange.bind(this)
        this.setElement = this.setElement.bind(this)
        this.onChangeInput = this.onChangeInput.bind(this)
        this.sumSelectedElements = this.sumSelectedElements.bind(this)
        this.submitSpell = this.submitSpell.bind(this)
    }

    submitSpell() {

        const spell = {
            Id: 0,
            Name: this.state.spellname,
            BaseDamage: parseInt(this.state.baseDamage),
            BaseManaCost: parseInt(this.state.baseManaCost),
            Element: this.sumSelectedElements(),
            Rules: [...this.props.rules.rules]
        }

        spell.Rules.forEach(r => r.value = parseInt(r.value))

        this.props.addSpellAction(spell)
    }

    setElement(event) {
        this.setState({
            selectedElements: event
        })
    }

    handleChange(event) {
        this.setElement(event.target.value)
    }

    onChangeInput(event) {
        this.setState({
            [event.target.name]: event.target.value
        })
    }

    sumSelectedElements() {
        return this.state.selectedElements.map(e => e.value).reduce((e1, e2) => e1 + e2)
    }

    render() {
        const { classes } = this.props
        return (
            <div className={classes.formWrapper}>
                <TextField 
                    id="name"
                    type="text"
                    value={this.state.spellname || ""}
                    label={APP_TEXTS.spellnameLabel[this.props.language.selected]}
                    required
                    name="spellname"
                    margin="dense"
                    onChange={this.onChangeInput}
                    fullWidth={true}
                />
                <div className={classes.numericInputWrapper}>
                    <TextField
                        className={classes.numericInput}
                        id="name"
                        type="number"
                        value={this.state.baseDamage || 0}
                        label={APP_TEXTS.baseDamageLabel[this.props.language.selected]}
                        required
                        name="baseDamage"
                        margin="dense"
                        onChange={this.onChangeInput}
                        fullWidth={false}
                    />
                    <TextField
                        className={classes.numericInput}
                        id="name"
                        type="number"
                        value={this.state.baseManaCost || 0}
                        label={APP_TEXTS.baseManaCostLabel[this.props.language.selected]}
                        required
                        name="baseManaCost"
                        margin="dense"
                        onChange={this.onChangeInput}
                        fullWidth={false}
                    />
                </div>
                <FormControl 
                    fullWidth={true}
                >
                    <InputLabel htmlFor="select-multiple-chip">
                        {APP_TEXTS.elementsText[this.props.language.selected]}
                    </InputLabel>
                    <Select
                        className={classes.menuItem}
                        multiple
                        value={this.state.selectedElements}
                        fullWidth={true}
                        onChange={this.handleChange}
                        input={<Input id="select-multiple-chip" />}
                        renderValue={
                            selected => (
                                <div>
                                {
                                    selected.map((i, k) => <Chip key={k} label={i.name[this.props.language.selected]} />)
                                }
                                </div>
                            )
                        }
                        >
                        {
                            elementList.map((i, k) => (
                                <MenuItem key={k} value={i}>
                                    {i.name[this.props.language.selected]}
                                </MenuItem>))
                        }
                    </Select>
                </FormControl>
                <div className={this.props.classes.addRulesComponentWrapper}>
                    <AddRules 
                    />
                </div>
                <Button 
                    disabled={!this.state.selectedElements.length || !this.state.spellname || !this.state.baseDamage || !this.state.baseManaCost}
                    variant="outlined"
                    color="primary"
                    onClick={this.submitSpell}
                    fullWidth={false}
                    text={APP_TEXTS.doneText[this.props.language.selected]}
                />
            </div>
        )
    }
}

const mapStateToProps = state => ({
    rules: state.rulesReducer,
    language: state.languageReducer
})

const mapDispatchToProps = dispath =>
    bindActionCreators({
        addSpellAction
    }, dispath)

export default withStyles(styles)(connect(mapStateToProps, mapDispatchToProps)(AddSpellComponent))