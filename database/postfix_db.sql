--
-- PostgreSQL database dump
--

-- Dumped from database version 9.3.10
-- Dumped by pg_dump version 9.3.10
-- Started on 2016-11-02 15:36:32

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

DROP DATABASE postfix;
--
-- TOC entry 1961 (class 1262 OID 17639)
-- Name: postfix; Type: DATABASE; Schema: -; Owner: -
--

CREATE DATABASE postfix WITH TEMPLATE = template0 ENCODING = 'UTF8';


\connect postfix

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 7 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA public;


--
-- TOC entry 1962 (class 0 OID 0)
-- Dependencies: 7
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON SCHEMA public IS 'standard public schema';


--
-- TOC entry 6 (class 2615 OID 17640)
-- Name: vmail; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA vmail;


--
-- TOC entry 177 (class 3079 OID 11750)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 1963 (class 0 OID 0)
-- Dependencies: 177
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = vmail, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 171 (class 1259 OID 17641)
-- Name: aliases; Type: TABLE; Schema: vmail; Owner: -; Tablespace: 
--

CREATE TABLE aliases (
    als_id integer NOT NULL,
    mbx_id integer,
    als_name character varying(128),
    als_created_by character varying(128),
    als_create_time timestamp with time zone,
    als_updated_by character varying(128),
    als_update_time timestamp with time zone
);


--
-- TOC entry 172 (class 1259 OID 17644)
-- Name: aliases_als_id_seq; Type: SEQUENCE; Schema: vmail; Owner: -
--

CREATE SEQUENCE aliases_als_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 1964 (class 0 OID 0)
-- Dependencies: 172
-- Name: aliases_als_id_seq; Type: SEQUENCE OWNED BY; Schema: vmail; Owner: -
--

ALTER SEQUENCE aliases_als_id_seq OWNED BY aliases.als_id;


--
-- TOC entry 173 (class 1259 OID 17646)
-- Name: domains; Type: TABLE; Schema: vmail; Owner: -; Tablespace: 
--

CREATE TABLE domains (
    dmn_id integer NOT NULL,
    dmn_name character varying(128),
    dmn_created_by character varying(128),
    dmn_updated_by character varying(128),
    dmn_create_time timestamp with time zone,
    dmn_update_time timestamp with time zone
);


--
-- TOC entry 174 (class 1259 OID 17649)
-- Name: domains_dmn_id_seq; Type: SEQUENCE; Schema: vmail; Owner: -
--

CREATE SEQUENCE domains_dmn_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 1965 (class 0 OID 0)
-- Dependencies: 174
-- Name: domains_dmn_id_seq; Type: SEQUENCE OWNED BY; Schema: vmail; Owner: -
--

ALTER SEQUENCE domains_dmn_id_seq OWNED BY domains.dmn_id;


--
-- TOC entry 175 (class 1259 OID 17651)
-- Name: mailboxes; Type: TABLE; Schema: vmail; Owner: -; Tablespace: 
--

CREATE TABLE mailboxes (
    mbx_id integer NOT NULL,
    dmn_id integer,
    mbx_username character varying(128),
    mbx_password character varying(128),
    mbx_create_time timestamp with time zone,
    mbx_created_by character varying(128),
    mbx_update_time timestamp with time zone,
    mbx_updated_by character varying(128),
    mbx_quota integer,
    mbx_is_admin boolean DEFAULT false NOT NULL,
    mbx_is_active boolean DEFAULT true NOT NULL
);


--
-- TOC entry 176 (class 1259 OID 17654)
-- Name: mailboxes_mbx_id_seq; Type: SEQUENCE; Schema: vmail; Owner: -
--

CREATE SEQUENCE mailboxes_mbx_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 1966 (class 0 OID 0)
-- Dependencies: 176
-- Name: mailboxes_mbx_id_seq; Type: SEQUENCE OWNED BY; Schema: vmail; Owner: -
--

ALTER SEQUENCE mailboxes_mbx_id_seq OWNED BY mailboxes.mbx_id;


--
-- TOC entry 1837 (class 2604 OID 17656)
-- Name: als_id; Type: DEFAULT; Schema: vmail; Owner: -
--

ALTER TABLE ONLY aliases ALTER COLUMN als_id SET DEFAULT nextval('aliases_als_id_seq'::regclass);


--
-- TOC entry 1838 (class 2604 OID 17657)
-- Name: dmn_id; Type: DEFAULT; Schema: vmail; Owner: -
--

ALTER TABLE ONLY domains ALTER COLUMN dmn_id SET DEFAULT nextval('domains_dmn_id_seq'::regclass);


--
-- TOC entry 1839 (class 2604 OID 17658)
-- Name: mbx_id; Type: DEFAULT; Schema: vmail; Owner: -
--

ALTER TABLE ONLY mailboxes ALTER COLUMN mbx_id SET DEFAULT nextval('mailboxes_mbx_id_seq'::regclass);


--
-- TOC entry 1843 (class 2606 OID 17660)
-- Name: aliases_pkey; Type: CONSTRAINT; Schema: vmail; Owner: -; Tablespace: 
--

ALTER TABLE ONLY aliases
    ADD CONSTRAINT aliases_pkey PRIMARY KEY (als_id);


--
-- TOC entry 1845 (class 2606 OID 17662)
-- Name: domains_pkey; Type: CONSTRAINT; Schema: vmail; Owner: -; Tablespace: 
--

ALTER TABLE ONLY domains
    ADD CONSTRAINT domains_pkey PRIMARY KEY (dmn_id);


--
-- TOC entry 1847 (class 2606 OID 17664)
-- Name: mailboxes_pkey; Type: CONSTRAINT; Schema: vmail; Owner: -; Tablespace: 
--

ALTER TABLE ONLY mailboxes
    ADD CONSTRAINT mailboxes_pkey PRIMARY KEY (mbx_id);


--
-- TOC entry 1848 (class 2606 OID 17665)
-- Name: fk_aliases_mailboxes; Type: FK CONSTRAINT; Schema: vmail; Owner: -
--

ALTER TABLE ONLY aliases
    ADD CONSTRAINT fk_aliases_mailboxes FOREIGN KEY (mbx_id) REFERENCES mailboxes(mbx_id);


--
-- TOC entry 1849 (class 2606 OID 17670)
-- Name: fk_mailboxes_domains; Type: FK CONSTRAINT; Schema: vmail; Owner: -
--

ALTER TABLE ONLY mailboxes
    ADD CONSTRAINT fk_mailboxes_domains FOREIGN KEY (dmn_id) REFERENCES domains(dmn_id);


-- Completed on 2016-11-02 15:36:33

--
-- PostgreSQL database dump complete
--

